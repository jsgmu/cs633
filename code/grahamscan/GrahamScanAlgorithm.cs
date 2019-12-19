using CompGeomVis.algotracking;
using CompGeomVis.canvas;
using CompGeomVis.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.grahamscan
{
    public class GrahamScanAlgorithm : AlgorithmBase
    {
        public List<Vector> SortedInput { get; set; }
        public Vector LowestPoint { get; set; }
        public PolygonModel Hull { get; set; }

        private const string SECTION_SORT_START = "sort_start";
        private const string SECTION_SORT = "sort";
        private const string SECTION_DEGENERATE = "degenerate";
        private const string SECTION_PRE = "pre";
        private const string SECTION_CLOCKWISE = "clockwise";
        private const string SECTION_COUNTERCLOCKWISE = "counterclockwise";
        private const string SECTION_NONE = "none";
        private const string SECTION_POST = "post";

        public GrahamScanAlgorithm()
        {
            AlgorithmId = 10;
            History = new AlgorithmHistory();
        }

        public Vector FindLowestPoint()
        {
            LowestPoint = null;

            foreach(var v in InputPoints)
            {
                if(LowestPoint == null)
                {
                    LowestPoint = v;
                }
                else if(v.Y < LowestPoint.Y || (v.Y == LowestPoint.Y && v.X < LowestPoint.X))
                {
                    LowestPoint = v;
                }
            }

            return LowestPoint;
        }

        public int GrahamSort(Vector v1, Vector v2)
        {
            if (v1 == v2 || (v1.X == v2.X && v1.Y == v2.Y))
                return 0;

            LowestPoint = FindLowestPoint();

            double thetaA = Math.Atan2(v1.Y - LowestPoint.Y, v1.X - LowestPoint.X);
            double thetaB = Math.Atan2(v2.Y - LowestPoint.Y, v2.X - LowestPoint.X);

            if (thetaA < thetaB)
            {
                return -1;
            }
            else if (thetaA > thetaB)
            {
                return 1;
            }

            double distanceA = GeomMath.Distance(LowestPoint, v1);
            double distanceB = GeomMath.Distance(LowestPoint, v2);

            if (distanceA < distanceB)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        private bool AllPointsCollinear()
        {
            if(InputPoints.Count <= 2)
            {
                return true;
            }

            Vector p1 = InputPoints[0];
            Vector p2 = InputPoints[1];

            for (int i = 2; i < InputPoints.Count; i++)
            {
                Vector p3 = InputPoints[i];

                if (GeomMath.GetTurnDirection(p1, p2, p3) != GeomMath.DIRECTION_NONE)
                {
                    return false;
                }
            }

            return true;
        }

        private void AddStackLinesToLayer(AlgorithmStatusLayer layer, Stack<Vector> stack)
        {
            var a = stack.ToArray<Vector>();

            for(int i = 0; i < a.Length - 1; i++)
            {
                var ap = a[i].Alternates;
                var ap2 = a[i+1].Alternates;

                if (ap == null || ap2 == null)
                    return;

                layer.AddCommand(new AddLineCommand
                {
                    AssociatedAlgorithm = this,
                    StartXIndex = ap.DotIndexLeft,
                    StartYIndex = ap.DotIndexTop,
                    EndXIndex = ap2.DotIndexLeft,
                    EndYIndex = ap2.DotIndexTop,
                    Comments = ""
                });
            }
        }

        public override void Run()
        {
            // Algorithm Setup: save input points
            var layer = History.CreateAndAddNewLayer("Setup (sorting input)");
            layer.AddCommand(new UpdatePointSetCommand
            {
                Label = "Input",
                Points = AlgorithmUtil.CopyVectorList(InputPoints)
            });
            layer.AddCommand(new HighlightLiveCodeSectionsCommand(AlgorithmId, SECTION_SORT_START, SECTION_SORT));

            SortedInput = new List<Vector>();

            // algorithm step 1: sort, saved sorted input points

            foreach(var v in InputPoints)
            {
                SortedInput.Add(new Vector { X = v.X, Y = v.Y, Alternates = v.Alternates });
            }

            SortedInput.Sort(GrahamSort);

            layer.AddCommand(new UpdatePointSetCommand
            {
                Label = "Sorted Input",
                Points = AlgorithmUtil.CopyVectorList(SortedInput)
            });

            // check degenerate case #1: 0,1,2 points
            layer = History.CreateAndAddNewLayer("Degenerate Case Checks");

            if (SortedInput.Count <= 2)
            {
                layer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Algorithm cannot execute, it requires at least 3 points"
                });
                layer.AddCommand(new HighlightLiveCodeSectionsCommand(AlgorithmId, SECTION_DEGENERATE));
                return;
            }

            // check degenerate case #2: all points on same line
            if(AllPointsCollinear())
            {
                layer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Algorithm cannot execute if all points are collinear"
                });
                layer.AddCommand(new HighlightLiveCodeSectionsCommand(AlgorithmId, SECTION_DEGENERATE));
                return;
            }

            // points cannot be collinear, points must be at least 3

            Stack<Vector> grahamStack = new Stack<Vector>();

            grahamStack.Push(SortedInput[0]);
            grahamStack.Push(SortedInput[1]);

            // starting points: first two sorted points, pushed onto stack

            // STATIC LAYER: "Starting Points", has only input points
            // layer commentary: if 0, 1 or 2 points, cannot perform graham scan
            //                   if all points on same line, cannot perform graham scan

            layer = History.CreateAndAddNewLayer("Initiailzation");
            layer.AddCommand(new AlgorithmStartingCommand { AssociatedAlgorithm = this });
            layer.AddCommand(new HighlightLiveCodeSectionsCommand(AlgorithmId, SECTION_PRE));
            var hip = new HighlightPointsCommand
            {
                AssociatedAlgorithm = this,
                HighlightLevel = 1
            };

            hip.Points.Add(new Vector { X = SortedInput[0].X, Y = SortedInput[0].Y });
            hip.Points.Add(new Vector { X = SortedInput[1].X, Y = SortedInput[1].Y });

            layer.AddCommand(hip);

            layer.AddCommand(new VectorProcessingStackUpdatedCommand
            {
                AssociatedAlgorithm = this,
                ProcessingStack = AlgorithmUtil.CopyVectorStack(grahamStack),
                Comments = "Initial stack"
            });
            AddStackLinesToLayer(layer, grahamStack);

            for (int i = 2; i < SortedInput.Count; i++)
            {
                layer = History.CreateAndAddNewLayer("Processing Point, index=" + i);

                layer.AddCommand(new AlgorithmStepStartingCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Step Starting"
                });

                layer.AddCommand(new ClearPointHighlightsCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = ""
                });

                layer.AddCommand(new ClearTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = ""
                });

                // NORMAL LAYER: "Algorithm Layer (i-2)"
                // Commentary: "We examine the next point in sorted order with the top two 
                //              elements from the stack status structure, popping the first one"
                // stack visualization: highlight top of stack, label it "tail"
                // standalone visualization: show SortedInput[i] as "head"
                //                           show popped element as "middle"
                // highlight the "head" point in yellow, and the "middle" / "tail" points in green
                // layer commentary: 

                // loop iteration "i"
                Vector head = SortedInput[i];
                Vector middle = grahamStack.Pop();
                Vector tail = grahamStack.Peek();

                layer.AddCommand(new HighlightInputPointCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Next input point",
                    X = head.X,
                    Y = head.Y,
                    HightlightLevel = 1
                });

                hip = new HighlightPointsCommand
                {
                    AssociatedAlgorithm = this,
                    HighlightLevel = 1
                };

                hip.Points.Add(new Vector { X = head.X, Y = head.Y });
                hip.Points.Add(new Vector { X = middle.X, Y = middle.Y });
                hip.Points.Add(new Vector { X = tail.X, Y = tail.Y });
                layer.AddCommand(hip);

                layer.AddCommand(new HighlightInputPointCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Freshly popped from top of stack",
                    X = middle.X,
                    Y = middle.Y,
                    HightlightLevel = 2
                });

                layer.AddCommand(new HighlightInputPointCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Top of stack, which is left on stack and peeked",
                    X = tail.X,
                    Y = tail.Y,
                    HightlightLevel = 2
                });

                // we examine next point in sorted list, with top element of stack (which we pop)
                // and 2nd element of stack (which we leave as new top of stack)

                // Commentary: "The turn direction of these three points is calculated using
                //              the cross product of these three points"
                int turn = GeomMath.GetTurnDirection(tail, middle, head);

                // determine "turn" of these 3 points, in sequence tail / middle / head
                string turnString = "Counter-clockwise";
                if(turn == GeomMath.DIRECTION_CLOCKWISE)
                {
                    turnString = "Clockwise";
                } else if(turn == GeomMath.DIRECTION_NONE)
                {
                    turnString = "None";
                }

                layer.AddCommand(new AddTextStatusCommand
                {
                    AssociatedAlgorithm = this,
                    Comments = "Computed turn: " + turnString
                });

                // Standalone visualization: "The turn direction for these three points is: "
                switch (turn)
                {
                    case GeomMath.DIRECTION_COUNTERCLOCKWISE:
                        layer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "The point popped is pushed back onto stack since it is part of the hull"
                        });
                        layer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "The input point is also pushed since it is potentially part of the hull"
                        });
                        layer.AddCommand(new HighlightLiveCodeSectionsCommand(AlgorithmId, SECTION_COUNTERCLOCKWISE));

                        grahamStack.Push(middle);
                        grahamStack.Push(head);

                        layer.AddCommand(new VectorProcessingStackUpdatedCommand
                        {
                            AssociatedAlgorithm = this,
                            ProcessingStack = AlgorithmUtil.CopyVectorStack(grahamStack),
                            Comments = "Updated processing stack"
                        });
                        break;
                    case GeomMath.DIRECTION_CLOCKWISE:
                        layer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "The point on the top of the stack is discarded, but the input point is preserved for re-consideration"
                        });
                        layer.AddCommand(new HighlightLiveCodeSectionsCommand(AlgorithmId, SECTION_CLOCKWISE));
                        i--;
                        break;
                    case GeomMath.DIRECTION_NONE:
                        layer.AddCommand(new AddTextStatusCommand
                        {
                            AssociatedAlgorithm = this,
                            Comments = "Input point is co-linear with other points, so it is part of the hull"
                        });
                        layer.AddCommand(new HighlightLiveCodeSectionsCommand(AlgorithmId, SECTION_NONE));

                        grahamStack.Push(head);

                        layer.AddCommand(new VectorProcessingStackUpdatedCommand
                        {
                            AssociatedAlgorithm = this,
                            ProcessingStack = AlgorithmUtil.CopyVectorStack(grahamStack),
                            Comments = "Updated processing stack"
                        });
                        break;
                }

                AddStackLinesToLayer(layer, grahamStack);
            }

            layer = History.CreateAndAddNewLayer("Final Results");
            layer.AddCommand(new AlgorithmCompleteCommand { AssociatedAlgorithm = this });
            layer.AddCommand(new ClearPointHighlightsCommand
            {
                AssociatedAlgorithm = this,
                Comments = ""
            });

            layer.AddCommand(new ClearTextStatusCommand
            {
                AssociatedAlgorithm = this,
                Comments = ""
            });
            layer.AddCommand(new HighlightLiveCodeSectionsCommand(AlgorithmId, SECTION_POST));

            grahamStack.Push(SortedInput[0]);

            layer.AddCommand(new VectorProcessingStackUpdatedCommand
            {
                AssociatedAlgorithm = this,
                ProcessingStack = AlgorithmUtil.CopyVectorStack(grahamStack),
                Comments = "Final stack, first input point is pushed to complete the hull"
            });
            AddStackLinesToLayer(layer, grahamStack);

            Hull = new PolygonModel();

            var a = grahamStack.ToArray<Vector>();

            for (int i = 0; i < a.Length - 1; i++)
            {
                var ap = a[i].Alternates;
                var ap2 = a[i+1].Alternates;

                if (ap != null && ap2 != null)
                {
                    // Main operation
                    Hull.Lines.Add(new LineModel
                    {
                        StartPoint = new Vector {
                            X = a[i].X,
                            Y = a[i].Y,
                            Alternates = new CanvasPoint { DotIndexLeft = ap.DotIndexLeft, DotIndexTop = ap.DotIndexTop } },
                        EndPoint = new Vector
                        {
                            X = a[i+1].X,
                            Y = a[i+1].Y,
                            Alternates = new CanvasPoint { DotIndexLeft = ap2.DotIndexLeft, DotIndexTop = ap2.DotIndexTop }
                        }
                    });
                } else
                {
                    // Side operation that doesn't involve points from grid
                    Hull.Lines.Add(new LineModel
                    {
                        StartPoint = new Vector { X = a[i].X, Y = a[i].Y },
                        EndPoint = new Vector
                        {
                            X = a[i+1].X, Y = a[i+1].Y
                        }
                    });
                }
            }
        }
    }
}

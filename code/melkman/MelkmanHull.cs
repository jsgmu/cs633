using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompGeomVis.melkman
{
    // This is based on code provided by professor as solution for assignment #1
    // This wasn't fully explored or implemented, leaving this here because it can be
    // completed and brought into the main tool in a few hours most likely
    //
    // This convex hull implemetation realizes the idea from 
    // A. Melkman, "On-line construction of the convex hull of a simple polygon", 
    // Info. Proc. Letters 25, 11-12 (1987)
    public class MelkmanHull : AlgorithmBase
    {
        //        inline ply_vertex * next(ply_vertex* v)
        //        {
        //            return v->getNext();
        //        }

        //        inline bool turn_left
        //        (const Point2d& p1,const Point2d& p2,const Point2d& p3)
        //{
        //    Vector2d v1 = p2 - p1;
        //        Vector2d v2 = p3 - p2;
        //    return ((v1[0]* v2[1]-v2[0]*v1[1])>0);
        //}

        private int currentVertexIndex = 0;

        private bool turn_left(Vector p1, Vector p2, Vector p3)
        {
            var v1 = GeomMath.Subtract(p2, p1);
            var v2 = GeomMath.Subtract(p3, p2);

            return (v1.X * v2.Y - v2.X * v1.Y) > 0;
        }
        
//    //update the top of the hull
//    inline void updateHullTop(ply_vertex* v, list<ply_vertex*>& hull)
//    {
//        typedef list<ply_vertex*>::iterator VIT;
//        if (hull.size() == 1) return;
//        VIT l1 = hull.end(); l1--;
//        VIT l2 = l1; l2--;
//        if (turn_left((*l2)->getPos(), (*l1)->getPos(), v->getPos()))
//            return;
//        hull.pop_back();
//        updateHullTop(v, hull);
//    }

        private void updateHullTop(Vector v, List<Vector> hull)
        {
            if (hull.Count == 1)
                return;

            Vector last = hull[hull.Count - 1];
            Vector secondToLast = hull[hull.Count - 2];

            if (turn_left(secondToLast, last, v))
                return;

            hull.RemoveAt(hull.Count - 1);
            updateHullTop(v, hull);
        }

//    //update the bottom of the hull
//    inline void updateHullBot(ply_vertex* v, list<ply_vertex*>& hull)
//    {
//        typedef list<ply_vertex*>::iterator VIT;
//        if (hull.size() == 1) return;
//        VIT b1 = hull.begin();
//        VIT b2 = b1; b2++;
//        if (turn_left((*b1)->getPos(), (*b2)->getPos(), v->getPos()))
//            return;
//        hull.pop_front();
//        updateHullBot(v, hull);
//    }

        private void updateHullBot(Vector v, List<Vector> hull)
        {
            if (hull.Count == 1)
                return;

            Vector b1 = hull[0];
            Vector b2 = hull[1];

            if(turn_left(b1, b2, v))
            {
                return;
            }

            hull.RemoveAt(0);
            updateHullBot(v, hull);
        }

//    //check if v is inside the hull
//    inline bool inHull(ply_vertex* v, list<ply_vertex*>& hull )
//    {
//        typedef list<ply_vertex*>::iterator VIT;
//        VIT t1 = hull.end(); t1--;
//        VIT t2 = t1; t2--;
//        if (!turn_left((*t2)->getPos(), (*t1)->getPos(), v->getPos()))
//            return false;

//        VIT b1 = hull.begin();
//        VIT b2 = b1; b2++;
//        return turn_left((*b1)->getPos(), (*b2)->getPos(), v->getPos());
//    }

        private bool inHull(Vector v, List<Vector> hull)
        {
            var t1 = hull[hull.Count - 1];
            var t2 = hull[hull.Count - 2];

            if (!turn_left(t2, t1, v))
                return false;

            var b1 = hull[0];
            var b2 = hull[1];

            return turn_left(b1, b2, v);
        }

        private void Hull2D()
        {

        }

//    //e mush be reachable from s
//    void hull2d(ply_vertex* s, ply_vertex* e, list<ply_vertex*>& hull )
//    {
//        //avoid letting s and e in the bridge
//        //if( s->getBridge()!=NULL ) s=s->getBridge()->v2;
//        //if( e->getBridge()!=NULL ) e=e->getBridge()->v1;
//        ply_vertex* ne = next(e);

//        //init the hull
//        hull.push_back(s);
//        hull.push_back(next(s));
//        hull.push_front(hull.back());

//        //incrementally create the hull
//        ply_vertex* ptr = next(hull.back());
//        while (ptr != ne)
//        {

//            //check if the ptr is contained in the hull
//            if (!inHull(ptr, hull))
//            {
//                updateHullTop(ptr, hull);
//                updateHullBot(ptr, hull);
//                hull.push_back(ptr);
//                hull.push_front(ptr);
//            }

//            ptr = next(ptr);

//        }//end while(ptr!=ne);

//        hull.pop_back();
//    }

        public override void Run()
        {

        }
    }
}

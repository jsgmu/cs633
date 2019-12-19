# Computational Geometry Algorithm Visualzation Prototype

This is the final project for CS633 at George Mason University, Fall 2019

This project is meant as an exploration of some computational geometry algorithms as well as a prototype of a tool for visualizing and exploring algorithms

If you have questions feel free to send an e-mail to jscanlo4@gmu.edu

This project is targetted to .NET Framework 4.7.2, it may work with other .NET framework versions but no guarantees are made

If you have any trouble building, the most likely cause is the Scintilla WPF DLL located in "lib". Fix this reference and it should build and run.

The 3rd party components used are:

- Scintilla / ScintillaWPF Wrapper
- Xceed WPF Toolkit (for advanced controls in User Interface)
- Newtonsoft Json (for serializing models to/from files on disk)

Additional code, such as saving of a WPF canvas to a PNG file, are from stackoverflow and other places on the Internet, consult source code for any links.

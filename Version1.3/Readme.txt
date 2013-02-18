AvalonDock 1.3 - Release Notes

AvalonDock.sln is the main solution of AvalonDock: it's a VS2010 solution and targets .NET 4.0. AvalonDock35.sln 
is a convenient solution used only fo recompiling AvalonDock for .NET 3.5. When switching betwen them much probably
a rebuild all for the solution is required. To completely remove any possible mismatch of referenced framework 
assemblies remove the obj directories of each project (just execute ClearObjs.bat). 

AvalonDock project is the main class library project. It contains all the sources of AvaloDock, plus other resources
like default styles and images.

AvalonDock.DemoApp is an application used primarly for test, it is not exposed into the distribuited setup.

AvalonDock.Themes contains two compiled themes (dev2010.xaml and ExpressionDark.xaml). It will contain more themes
in the future.

Samples directory contains four sample projects that use AvalonDock. These samples show some features of AvalonDock
and are included into the setup. To compile properly samples project be sure to compile a release version of 
AvalonDock project.

Copyright (c) 2007-2010, Adolfo Marinucci
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, 
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, 
  this list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice, 
  this list of conditions and the following disclaimer in the documentation 
  and/or other materials provided with the distribution.
* Neither the name of Adolfo Marinucci nor the names of its contributors may 
  be used to endorse or promote products derived from this software without 
  specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, 
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, 
EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
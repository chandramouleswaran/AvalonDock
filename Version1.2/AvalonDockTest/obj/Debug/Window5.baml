   M S B A M L   `   `   `  ����T  PAvalonDock, Version=1.2.2702.0, Culture=neutral, PublicKeyToken=85a1e0ada7ec13e4;,clr-namespace:AvalonDock;assembly=AvalonDock
AvalonDock  5       AvalonDockTest   AvalonDockTest.Window5   5   	   W SPresentationCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      8x,http://schemas.microsoft.com/winfx/2006/xaml    5      9ad,clr-namespace:AvalonDock;assembly=AvalonDock    5        � Title$  Window5��5      $	��300��6   $	��300��6"   .��5      ]� ��.  � LayoutTransform 5      �� 5       �� ScaleX$ 1Q�6    �� ScaleY$ 1Q�6   .+�5
      o� $	��Top=�6	   .@�5      m� $
��File��6   .@�5      m� -   5         m�Name$ mnuFlipContent��5      $��Flip Content��6'    m� IsCheckable . 6P   5      5      �� 5      -   5         ��Name$ TestContainer��5      .��5         AvalonDock.DockingManager  -   5          Name$ _dockingManager��5      .��5         AvalonDock.ResizingPanel     Orientation$ Vertical=�6   .+�5        $ 
Horizontal=�6   .+�5         AvalonDock.DockablePane  	   ResizeHeight$	 * �6   .@�5   	       AvalonDock.DockableContent  -   5        
  Name$
 content1��5      .��5   
   �� .��6   
Content16   5   
   5   	     5      .@�5   	     -   5        $
 content2��5      .��5   
   �� .��6   
Content26   5   
   5   	     5      .@�5   	     -   5        $
 content3��5      .��5   
   �� .��6   
Content36   5    
   5!   	   5"        5#      -   5          Name$ bottomPanel��5#   _   $ 
Horizontal=�6   $
��Auto��61      ResizeWidth$ * �6>   .+�5$        $ * �6   .@�5%   	     -   5        $��ShowMeFirst��5%   0      AvalonDock.ManagedContent   Title$ FirstWindow��6   $y�	Collapsed=�6C   $ * �6Z   .��5&   
   �� .��6   This should be visible 1st6.   5'   
   5(   	     5)      $ * �6   .@�5*   	     -	   5        $��ShowMeSecond��5*   1   $ SecondWindow��6   $y�	Collapsed=�6E   $ * �6\   $
��Auto��6}   .��5+   
   �� .��6   This should be visible 2nd6.   5,   
   5-   	     5.      $ * �6   .@�5/   	     -
   5        $��AlsoShowMeSecond��5/   0   $ ThirdWindow��6   $y�	Collapsed=�6H   $ * �6_   $
��Auto��6�   .��50   
   �� .��6   $"This should be visible 2nd as well66   51   
   52   	   53      54      55      56      57      59      
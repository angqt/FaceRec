   M S B A M L   `   `   `  ���� H  DKinectSample1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:KinectTrackingKinectTracking      KinectTracking.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:KinectTracking     c _Microsoft.Expression.Drawing, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35=ed4http://schemas.microsoft.com/expression/2010/drawing    � Title$  Kinect Body Track��$	��575��$	��934�� � Topmost . .��� $��	0,0,327,0q� � ColumnDefinitions �� $
��335* ��� $	��92* �.+�]� $��2��.+�o� $	��Top=�$��
0,0,-331,0q�.@�m� $��_File��.@�m� -   $��_Open��m� $��_Logs��.@�m� -   $��_Open log folder��m� -   $��Show Coordinates Log��m� -   $��Show Orientation Log��m� -   $��_Exit Application��m� $��_Camera��.@�m� -   $��	_Log Face��m� -   $!��_Toggle gesture recognition��m� -   $��_Toggle color camera��m� -	   $��_Skeleton replay��m� $��_Data Processing��.@�m� -
   $��_Extract marked gestures��m� -   $*��$_Calculate bezier curve parameters..��m� $��_Gesture recognition��.@�m� -   $��_Recognise gestures..��m� -   $ ��_Recognise current gesture��m� -   $��_Set Marker count���� �� -    ��Name$ camera��$��	0,20,0,83q�$��2���� -    ��Name$ canvas��$��	0,20,0,83q�$��2���� -   $ topProjectionCanvas��$��130,19,-326,304q�$��1���� -   $ sideProjectionCanvas��$��130,248,-326,75q�$��1���� -   $��Buttoncanvas��$��
5,470,-5,4q�$��2��.+�0  *Microsoft.Expression.Shapes.RegularPolygon  ���	#FFF4F4F5$��48��   InnerRadius$ 1Q� �� Left$ 11��   
PointCount$ 3Q� �� Stretch$
 Fill=�
���   �
	 �� Top$	 11��$��48��
 J� RenderTransformOrigin$
 0.5,0.5&�z�V� .~��� �� ��  �� Angle$ 90Q�T� F� O������= � �0� � �0�`+����=`+�� �  � �iU�  � �iU� `+�� � `+�	���	#FFF4F4F5$��50��$ 64.334��$
 Fill=�
���   �$	 10��$��55.666���� -    ��Name$ record_button��$��record_Start��$��Right=�$��Bottom=�$��50��$��50�� J� Opacity$ 0Q�$ 10��$	 10���� -   $ stop_record_button��$��stop_record��$ 65��$	 10��$��55��$��50��$ 0Q��� -   $ screenshot_button��$��
screenshot��$ 515.6��$	 10��$��55��$��50��$	 100Q��� -    ��Name$ textBlockFrameNr��$	 117��$
��Wrap=�$	 10��$��50��$	��108��$��26.667(�$y�	Collapsed=��� -   $ 	textBlock��$ 10��$
��Wrap=�$��	Frame nr:��$	 10��$��50��$	��105��$��24(�$y�	Collapsed=��� -    ��Name$ label��$��marker:��$	 185��$	 9��$��26.667(��� -   $ markerCount��$
��0(3)��$	 290��$	 9��$��26.667(��� -   $ recognised_gesture��$�� ��$	 388��$	 10��$��20(�$	��512��A� -    A�Name$ recordBlinker�����	#FFF70202$
��Left=�$��45��$��140,480,0,0q�
���   �$	��Top=�$��45��$y�	Collapsed=��� $��1��$
��Left=�$	��100��$��125,195,-331,0q�$	��Top=�$	��335���� $
��Left=�$	��100��$��324,232,-253,0q�$	��Top=�$	��528��$
 0.5,0.5&�$��2��z�V� .~��� �� �� $ 90Q�T� 
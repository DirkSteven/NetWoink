   M S B A M L   `   `   `  ����I  ENetworkMonitor, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:NetworkMonitorNetworkMonitor  5   	   %    NetworkMonitor.InitializeWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:NetworkMonitor     5   	     � WindowStartupLocation$  CenterOwner=�5   	    � Title$ NIC Selection��5	   	   $	��340��6   $	��545��6,    � WindowState$ Normal=�5
   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � Icon$ /Resources/netwoink.ico��5   	    � 	Resources 5         NormalBrush �� 
StartPoint �� EndPoint	 � Color
 � Offset  WindowBackgroundBrush �� Color  ComboBoxToggleButton �� 
TargetType � ColumnDefinitions ��Name �� CornerRadius F�Name  ComboBoxTextBox �� 
TargetType �� Property �� Value �� 	ClickMode d� 	IsChecked �� Path �� Mode �� RelativeSource J� IsHitTestVisible �� SelectionBoxItem ��Name �� 
IsReadOnly �� 
IsReadOnly �� IsDropDownOpen � AllowsTransparency  J� SnapsToDevicePixels! � ActualWidth" �� MaxDropDownHeight# K� IsItemsHost-  'System.Windows.Input.KeyboardNavigation$   DirectionalNavigation% �� Triggers��%C	  &	        &	 �     &	 �     7   7  &	 �    '�� �    7  7  7  '�� u    '�� �    '�� f    �� 5   
   $	 0,0&�62   $	 0,1&�6C   ��5      � � 5      $	 #323030��6#   $	
 0.0Q�63   � 5      $	 #625E5E��6#   $	
 1.0Q�63   5      5      �� 5   
   $ #625E5E��68   �� 5   
   " d�67   .R�5      �  5      �� 5      �� 5       $��20 �6'   .+�5"      �� $ Border��6   $��2��6)   $ 2��6=   8��  $��1q�5$      �� 5%      $��0��5&      $ 2,0,0,2��5'      $��1q�5(      8�� $��0,0,1,0q�5*      F� 5+      $ Arrow��5,      $��1��5-      $��Center=�5.      $��Center=�5/      "����        � 	=  	= � z     	50      51      52      �� 54   
   " ��62   .R�55      �� $ PART_ContentHost��6   $|�False��6/   #��z��6A   56      �� 59   
   " ��6*   .�5:      ��  � �� 5;      �� 5<      " ��6&   .R�5=      � .+�5>      d� $��ToggleButton��5?      8��  $��2��5A      $|�false��5B      $ Press=�5D       5C      �� $ IsDropDownOpen�$ TwoWay=� �� *TemplatedParent+5E      �� 5F      $��ContentSite��5H      $ False��5I      $��3,3,23,3q�5J      $��Center=�5K      $
��Left=�5L      
�������#��z 5N      �� 5S      $ PART_EditableTextBox��6&   8�� $
��Left=�5V      $��Center=�5W      $��3,3,23,3q�5X      $
|�True��5Y      
�������$y�Hidden=�5[      # z 5\      
���������5T      Z� 5]      � 5_      $��Popup��5`      $��Bottom=�5a      #��z 5b      $
 True��5c      $|�False��5d      $��Slide=�5e      .��5f   "   � $��DropDown��5g   	   $
  True��5h   	   #��z! 5i   	   #��z" 5j   	   .+�5k   &   �� $ DropDownBorder��5l      8�� $��1q�5n      �� 5o   &   $��4,6,4,6q�63   $
  True��6D   .��5p   *   �� $
# True��65   $$ 	Contained=�6H   5q   '   5r   #   5s      5t      5u      5w      % 5x      5|      �� 5}   
   " ��6   .�5~      ��  � �� �#FFFFFF6+   5      �� 5�   
   " ��6   .�5�      ��  � ��$ 	Open Sans)�6+   �� 5�       �@K�ZIndex$ 1��6-   �� 5�       � �� �#ffffff6+   5�      �� 5�   
   " ��6   .�5�      ��  � ��$ 	Open Sans)�6+   �� 5�       � �� �#ffffff6+   5�      5�      .��5�      �� & �� Padding$& 10q�6   ���#323030._�5�   
   �� .+�5�      � $��0 0 0 20q�6    5�      �� 5�      $	��120 �6'   �� 5�      $��* �6'   .+�5�      �� $��0��6   $��Select An Adapter:��6(   �� 5�      -   5        ' ��Name$' comboBox��5�      ( �� DisplayMemberPath$
( Name��6-   $��1��6s   5�      �� 5�      -   5        ) ��Name$) nicTypeLabel��5�      $��	NIC Type:��6*   �� 5�      -   5        $) ipAddressLabel��5�      $��IP Address:��6,   �� 5�      -   5        $) macAddressLabel��5�      $��MAC Address:��6-   �� 5�      -   5        $) gatewayLabel��5�      $��Gateway:��6*   �� 5�      -   5        $) networkSSIDLabel��5�      $��Network SSID:��6.   �� 5�      -   5        $) driverVersionLabel��5�      $��Driver Version:��60   $��0 0 0 20q�6J   � 5�      $��
300 0 0 20q�6    5�      �� 5�      $��73 �6'   �� 5�      $��87 �6'   .+�5�      �� -   5        * ��Name$* 
QuitButton��5�      $��0��6G   $��0 0 0 0q�6W   
������ 
������ $��0,0,0,63q�5�   L   .��5�      �� + �� Orientation$+ 
Horizontal=�6!   .+�5�      �� $!��/Resources/color_cancel.png��6    $��34��6E   $��33��6Q   �� 5�      $
��Bold"�6$   $��40��66   , �� Padding$, 	10 10 0 0q�6A   $��0 0 0 0q�6U   Quit6l   5�      5�      �� 5�      -	   5        $* OKButton��5�      $��1��6C   $��0 0 0 0q�6S   
������ 
������ $��0,0,0,63q�5�   L   .��5�      �� $+ 
Horizontal=�6!   .+�5�      �� $��/Resources/color_ok.png��6    $��34��6A   $��35��6M   �� 5�      $
��Bold"�6$   $��28��66   $, 0 10 0 0q�6A   $��0 0 0 0q�6T   OK6i   5�      5�      �� 5�      $��/Resources/manicon.png��6   $��-300,-173,122,19q�68   - K� ZIndex$- -1��6R   5�      5�      5�      5�      
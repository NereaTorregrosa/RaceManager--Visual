   M S B A M L   `   `   `  ����T  Pprojecte3_TorregrosaNerea, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:projecte3_TorregrosaNerea.Viewsprojecte3_TorregrosaNerea.Views  5      1    +projecte3_TorregrosaNerea.Views.LiveResults   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:projecte3_TorregrosaNerea.Views     5        N� 
Background
  ����� N� Title$ LiveResults��5
      .��5      �  � RowDefinitions 5   
   �� 5      $
��0.5* �6   6,   �� 5      6   �� 5      $
��0.2* �6   6,   .+�5   
   �� $��0��6    �� Orientation$ 
Horizontal=�6"   .+�5      �� $ Vertical=�6   $��Center=�60   $��10q�6K   .+�5      �� $ 
Horizontal=�6   .+�5      �� $
��Nom:��6    6.   �� 5      -   5         ��Name$ txbFiltreNom��5      $��30��64   $	��200��6@   $��	17,0,0,10q�6L   6a   5      �� 5      $ 
Horizontal=�6   .+�5      �� $��Dorsal:��6    61   �� 5      -   5        $ txbFiltreDorsal��5      $��30��67   $	��200��6C   $��10,0,0,0q�6O   6c   5      5      �� 5      -   5         ��Name$ 
btnFiltrar��5      $��30��6)   $	��100��65   $��Filtrar��6A   $��20,0,0,0q�6S   6�   �� 5      -   5        $ btnNetejarFiltre��5      $��30��6/   $	��100��6;   $��Netejar filtre��6G   $��10,0,0,0q�6`   6�   5      � 5    
   $��1��6    � ColumnDefinitions 5!      �� 5"      6%   �� 5#      6%   .+�5%      &   System.Windows.Controls.DataGrid  -   5          Name$ 	dgResults��5%   '   $��0��6      AutoGenerateColumns$ False��6:   	   HeadersVisibility$	 Column=�6V   
   CanUserAddRows$
 False��6q      CanUserDeleteRows$ False��6�      Columns 5&      4  .System.Windows.Controls.DataGridTemplateColumn  5'      ,  &System.Windows.Controls.DataGridColumn   Header$ Status��6-      CellTemplate 5(      �� 5)      .R�5*   "   �� -   5        $ 
Horizontal=�5*   -   
 � Tag 6a   �� 5+   #   5,      5.      0  *System.Windows.Controls.DataGridTextColumn  50      $ Temps��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6o   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *Temps+ �� StringFormat$ {0:HH:mm:ss}��6o     51      $ Dorsal��6)   $ 50 6_    69   �� *DorsalParticipant+6_     52      $	 Nom��6)   $	 150 6Y    66   �� *NomParticipant+6Y     53      $ Action��6-    54      �� 55      .R�56   "   �� $ btnViewDetail��6)   -   5        $
��View��56   @   $��75��6O   6Z   57      59         RowStyle 5;      �� 5<       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6    �� Triggers 5=      �� 5>       �� Binding 6*   �� *	IsRetirat+ �� Value True6H   .a�5?   "   ��  �� Property � �� �� Value
 �  ��6?   5@      5B      5D      �� 5E      
���   �$��1q�6)   $��1��6=   $��5q�6M   $	��300��6X   $	��200��6d   ._�5F      � -   5         �Name$ grdDetallRegistre��5F      $	��Top=�62   $	��300��6J   $	��200��6V    5G      �� 5H      $
��Auto �6(   �� 5I      $
��Auto �6(   .+�5L      �� $��0��6!   $ 
Horizontal=�6.   $��10q�6G   $��Center=�6S   .+�5M      �� $
��Nom:��6$   $
��Bold"�60   $��0,0,5,0q�6B   �� 5N      -	   5         ��Name$ txbNomParticipant��5N   $   $��0,0,20,0q�6?   �� 5O      $��Dorsal:��6$   $
��Bold"�63   $��0,0,5,0q�6E   �� 5P      -
   5        $ txbDorsalParticipant��5P   $   $��0,0,20,0q�6B   5Q      �� 5S      -   5         ��Name$ lsvCheckpoints��5S      $��1��67   $��10q�6D   $��Center=�6P   $	��Top=�6m   $	��200��6�    �� View 5T      �� 5U      .P�5V   "   �� $��
Checkpoint��61    �� DisplayMemberBinding 6E   �� *KmCheckpoint+�� 5W   "   $��Temps��61    6@   �� *Temps+$ {0:HH:mm:ss}��5X      5Z      5[      5\      5]      �� 5^   
   -   5        $ btnTornarAConsultarCurses��5^      $��2��64   $��30��6A   $	��150��6M   $��Right=�6Y   $��Tornar a Consultar curses��6u   $��0,0,10,0q�6�   6�   5_      5`      
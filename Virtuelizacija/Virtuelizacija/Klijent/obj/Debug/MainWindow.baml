   M S B A M L   `   `   `  ����B  >Klijent, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:KlijentKlijent  5   	       Klijent.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Klijent     5   	     � Title$  
MainWindow��5   	   $	��670��6   $	��800��6)   .��5	      � 
�������.+�5
   
   �� $I��CEvidencija prognozirane i ostvarene potrošnje električne energije��6    �� Padding$ 10 5 5 5q�6]   $��
Constantia)�6p   $��20(�6�   $��0,36,0,0q�6�   $
��Bold"�6�   $��Center=�6�   �� 5   
   -   5         ��Name$ dodajSliku_Button��5      $
��Left=�6Q   $	��Top=�6l   $��
86,130,0,0q�6�   $��50��6�   $��75��6�   .��5      �� $��IZABERI PUTANJU��6   $
��Wrap=�6/    �� TextAlignment$ Center=�6C   $��
Constantia)�6Z   $��16(�6r    J� RenderTransformOrigin$ 0.953,0.572&�6�   5      �� 5   
   -   5         ��Name$ TextBlockPutanja��5      $
��Left=�6.   $	��Top=�6I   $ 10 15 30 30q�6a   $��214,107,0,0q�6w   $��50��6�   $	��538��6�   �� 5   
   -   5        $ ButtonPokreniCitanje��5      $��Center=�6S   $	��Top=�6p   $��	0,234,0,0q�6�   $��50��6�   $	��176��6�   $��POKRENI CITANJE��6�   $��
Constantia)�6�   $��16(�6�   �� 5   
   -   5        $ TextBlockGreska��5      $
��Left=�6-   $	��Top=�6H   $ 10 15 30 30q�6`   $��214,168,0,0q�6v   $��50��6�   $	��538��6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ ViewDataGrid��5         
IsReadOnly .    GridLinesVisibility$ 
Horizontal=�6<   	   AutoGenerateColumns	 .  $��113,307,112,183q�5   B   $ 1.372,-0.075&�6[   
   Columns
 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Id��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   ��  �� Path$ Id�  5      $ 	TimeStamp��6%    68   �� $ 	Timestamp�  5      $ ForecastValue��6%    6<   �� $ ForecastValue�  5      $ MeasuredtValue��6%    6=   �� $ MeasuredtValue1�  5      $! AbsolutePercentageDeviation��6%    6J   �� $! AbsolutePercentageDeviation�  5      $ SquaredDeviation��6%    6?   �� $ SquaredDeviation�5        5   
   -   5        $ ViewDataGrid1��5       . $ 
Horizontal=�6=   	 .  $��113,476,400,39q�5   B   
 5         5!      $ Id��6%    61   �� $ Id�  5"      $ 	Timestamp��6%    68   �� $ 	Timestamp�  5#      $ Message��6%    66   �� $ Message�  5$      $ MessageType��6%    6:   �� $ MessageType�5'        5)   
   -   5        $ ViewDataGrid2��5)       . $ 
Horizontal=�6=   	 .  $��405,476,112,39q�5*   B   
 5+        5,      $ Id��6%    61   �� $ Id�  5-      $ FileName��6%    67   �� $ FileName�5/      51      52      
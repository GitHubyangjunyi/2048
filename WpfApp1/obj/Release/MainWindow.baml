   M S B A M L   `   `   `  ���� ?  ;2048, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1      WpfApp1.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:WpfApp1       � Title$
  2048��$	��540��$	��420�����#FEFEEE.��� -    �Name$ mainGrid��$	��400��$��0,3,3,3q�$��Center=�$��Bottom=�$	��400�� � ShowGridLines .   � ColumnDefinitions �� �� $	��100 ��� $	��100 ��� $	��100 � � RowDefinitions �� $	��100 ��� $	��100 ��� $	��100 ��� $	��100 �.+��� -    ��Name$ topLabel��$��	Game Over��$
��Left=�$��	62,50,0,0q�$��1��$	��Top=�$��4��$	��100��$��2��$	��279��$��50(�$��Center=�$��Center=�
������� K� ZIndex$ 1��
��� E��$y�Hidden=� � -     �Name$	 fm1��$��4��$
��Left=�$��4��$	��Top=�$ 1��$��4q�
�����������  �� ElementName$ mainGrid��	 �� Path$	 Height�
 �� UpdateSourceTrigger$
 PropertyChanged=����� $ mainGrid��$	 Width�$
 PropertyChanged=��� -   $ 	lbl_score��$��2��$��0��$��15(�$��1q�
�������$��30��$��62,-42.6,1.2,0q�$��Center=�$	��Top=�$��Center=�$��136.8���� $��Score: ��$��15(�$��Center=�$��3.8,-42.6,0,0q�$	��Top=�$
��Left=�$��64���� $��Record: ��$
��Left=�$��15(�$��Center=�$��3.8,-42.6,0,0q�$	��Top=�$��80��$��2���� -   $ 
lbl_record��$��2��$��0��$��15(�$��1q�
�������$��30��$��63.8,-42.6,5.2,0q�$	��Top=�$��Center=�$��2���� -   $
��sld1��$
��Left=�$��
90,-89,0,0q�$	��Top=�$��3��$	��132��$��23�� � Minimum$ 4Q� � Maximum$ 8Q� �� TickFrequency$ 1Q� �� IsSnapToTickEnabled . |�.  �� $��2��$
��Left=�$��28��$��
27,-93,0,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��20��$��Center=� J� RenderTransformOrigin$ -1.387,0.626&����� $
 sld1��$	 Value�$
 PropertyChanged=��� $��Board size: ��$��15(�$��Center=�$��	4,-93,0,0q�$	��Top=�$
��Left=�$��81���� $��To restart: Press Esc��$��2��$
��Left=�$��73.933,-93.417,0,0q�$	��Top=�$��113.6��$
��30.4��$��2��
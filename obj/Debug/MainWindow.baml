   M S B A M L   `   `   `  ����E  AEncryption, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Encryption
Encryption  5   	       Encryption.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Encryption     5   	     � 
ResizeMode$  NoResize=�5   
    � Title$ 
MainWindow��5	   	   $	��720��6   $
��1280��6)   .��5
      � 
�������.+�5   
   �� -   5         ��Name$ ChooseImageButton��5      $��	10,10,0,0q�6,   $
��Left=�6?   $	��Top=�6Z   $��32��6r   $	��220��6~   $+��%Выбрать изображение��6�   $��20(�6�   
�������6�   � 5   
   -   5         �Name$ Buttons��5      $��13,54,1084,551q�6    .+�5      	� -   5         	�Name$ Encrypt��5      $��0,0,0,45q�6+   $��Шифрование��6=   $��20(�6R   $��Center=�6`    d� 	IsChecked$
 TrueY�6�    	� 	GroupName$
 Task��6�   $��40��6�   	� 5      -   5        $ Decrypt��5      $��0,45,0,0q�6+   $��Дешифрование��6=   $��20(�6T   $��Center=�6b   $��40��6�   $
 Task��6�   5      � 5   
   $��13,144,1129,411q�6   
�������.+�5      �� $��Ключ��6   $
��Left=�6#   $��30��6>   $��	10,10,0,0q�6J   $	��Top=�6]   $	��112��6u   $��5,0,0,0q�6�   $��20(�6�   �� 5      -   5         ��Name$ Colors��5      $
��Left=�6'   $��32��6B   $��	10,45,0,0q�6N   $	��Top=�6a   $	��100��6y   $��16(�6�   .@�5      ��  �� 
IsSelected . $	��rgb��61   6A   �� 5      $	��rbg��6   6/   �� 5      $	��gbr��6   6/   �� 5      $	��bgr��6   6/   �� 5      $	��grb��6   6/   �� 5      $	��brg��6   6/   5      �� 5      -   5        $ 	Direction��5      $
��Left=�6*   $	��Top=�6E   $��	10,82,0,0q�6]   $��32��6p   $	��100��6|   $��16(�6�   .@�5      ��  . $��00100111��61   6F   �� 5      $��11011000��6   64   �� 5      $��10110001��6   64   �� 5      $��01001110��6   64   5      5       �� 5!   
   -   5        $ StartButton��5!      $��
Старт��6&   $
��Left=�66   $��
13,285,0,0q�6Q   $	��Top=�6e   $��87��6}   $��31��6�   $��20(�6�   ���	#FFDEDEDE6�   �� 5"   
   -   5        	 ��Name$
	 Text��5"      $
��Left=�6    $��
13,321,0,0q�6;   
 �� TextWrapping$

 Wrap=�6O   $	��Top=�6c   $	��350��6{   $	��322��6�   $��16(�6�   �� 5#   
   $��340,10,10,10q�6   ���	#FFA6A6A6$��1q�6?   
�������._�5$      � .+�5%      �� -	   5         ��Name$ ImageContainer��5%      $��Center=�60   $��0,0,0,0q�6M   $��Center=�6^   5&      5'      5)      5*      
   M S B A M L   `   `   `  ����Q  MRecuperacionBiblioteca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:RecuperacionBiblioteca.ViewRecuperacionBiblioteca.View  5   	   +    %RecuperacionBiblioteca.View.LoginView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:RecuperacionBiblioteca.View     5   	      
background#��[  5   	     � Title$  	LoginView��5	   	   $	��350��6   $	��250��6(    � 	Resources 5        StyleButton �� 
TargetType �� Property  primarycolor �� Value  	fontWhite��%�   &	       7  7  �� 5   
   " ��6$   .�5      ��  � ��8   68   �� 5       � ��8  68   �� 5       � ��$ 15(�66   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   #��[ 6"   $��0px, 5px, 0px, 5pxq�5      .+�5      �� $��INICIAR SESIÓN��6   $��Center=�5      $
��10pxq�5      #��[ 5       5!      �� 5#   
   $��1��6   $	��5pxq�6"   .+�5%      �� $��Usuario��6   �� 5&      $��1��6   $��1��5'      $��0 0 0 10q�5)      ��5(      �� *
Username+5)      �� 5,      $��Contraseña��6   G� 5-      -   5        $��2��5-      $��1��5.      $��0 0 0 10q�50      51      �� 53   
   $��3��6   $��Bottom=�54      .+�55      �� $��Enviar��6   $��5, 5, 0,  5q�56      $��50, 10q�57      $��Right=�58      #��[ 59      ��5:      �� *LoginCommand+5<      5=      5>      5?      
   M S B A M L   `   `   `  ����Q  MRecuperacionBiblioteca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:RecuperacionBiblioteca.ViewRecuperacionBiblioteca.View  5   	   5    /RecuperacionBiblioteca.View.BibliotecaAdminView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:RecuperacionBiblioteca.View     5   	     � Title$  BibliotecaAdminView��5   	   $	��450��6%   $	��800��62   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $
��Auto��6.      primarycolor#��[  6;   .+�5      �� $��Añadir nuevo libro��6   $��5q�6Q   ��62   �� *
GoToCreate+6^   �� 5      $��Editar libro��6   $��5q�6Q   ��6,   �� *EditLibroCommand+6^   �� 5      $��Eliminar libro��6   $��5q�6U   ��6.   �� *DeleteLibroCommand+6b   �� 5      $��Logout��6   $��5q�6H   ��6&   �� *LogoutCommand+6U   �� 5      $��Descargar todo��6   $��5q�6P   ��6.   �� *ReportCommand+6]   5      �� 5   
   $��1��6   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ datagridLibros��5         AutoGenerateColumns$ False��5      $��Stretch=�5      $��Stretch=�5         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5         
IsReadOnly$
 True��5          CanUserAddRows$ False��5!         CanUserDeleteRows$ False��5"      ��5      �� *Libros+	 �� SelectedItem	 5      �� *LibroSeleccionado+
   	CellStyle
 5#      �� 5$       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6   .�5%      ��  �� Property � �� �� Value$ Center=�6G   �� 5&       � ��$ Center=�6E   �� 5'       � ��  
background# [ 6D   5(         Columns 5*      0  *System.Windows.Controls.DataGridTextColumn  5+      ,  &System.Windows.Controls.DataGridColumn   Header$ Id��6)   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *Id+  5,      $ Título��6)    69   �� *Titulo+  5-      $ Autor��6)    68   �� *Autor+  5.      $ Género��6)    69   �� *Genero+  5/      $
 Año��6)    66   �� *Anio+  50      $
 ISBN��6)    67   �� *Isbn+  51      $ Sinopsis��6)    6;   �� *
Sinopsis+4  .System.Windows.Controls.DataGridTemplateColumn  52      $ Imagen��6-      CellTemplate 53      �� 54      .R�55   "   �� $��30��6B   ��6(   �� *Imagen+6B   56      58      5:      5;      5<      5=      
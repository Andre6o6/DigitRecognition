   M S B A M L   `   `   `  ����K  GDigitRecognition, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DigitRecognitionDigitRecognition  5   	   -    'DigitRecognition.NetConfigurationWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:DigitRecognition     5   	     � Title$  Change configuration��5   	   $	��328��6&   $	��375��63   .��5	      � $��0,0,2,0q�6   .+�5
   
   �� -   5         ��Name$ label��5
      $��Activation function:��6   $
��Left=�6>   $��	10,10,0,0q�6Y   $	��Top=�6l   �� 5   
   -   5         ��Name$ ActivationFuncComboBox��5      $
��Left=�63   $��
127,14,0,0q�6N   $	��Top=�6b   $	��120��6z   .@�5      �� $��Sigmoid��6   �� 5      $
��Tanh��6   �� 5      $
��ReLU��6   5      �� 5   
   -   5        $ label1��5      $��Coefficient:��6    $
��Left=�67   $��	10,36,0,0q�6R   $	��Top=�6e   �� 5   
   -   5         ��Name$ coefficientTextBox��5      $
��Left=�6.   $��23��6I   $��
127,36,0,0q�6U    �� TextWrapping$
 Wrap=�6i   $��1��6}   $	��Top=�6�   $��28��6�   �� 5   
   -   5        $ label2��5      $��Hidden layers:��6    $
��Left=�69   $��	10,67,0,0q�6T   $	��Top=�6g   �� 5   
   -   5        $ layerCountTextBox��5      $
��Left=�6-   $��23��6H   $��
104,68,0,0q�6T   $
 Wrap=�6h   $��1��6|   $	��Top=�6�   $��28��6�    J� RenderTransformOrigin$ 0.452,0.652&�6�   6�   �� 5   
   -   5         ��Name$ button1��5      $��OK��6"   $
��Left=�6/   $��
104,0,0,10q�6J   $��75��6^   $��20��6   $��Bottom=�6�   �� 5   
   -   5        $ button2��5      $��Cancel��6"   $
��Left=�63   $��
184,0,0,10q�6N   $��75��6b   $��20��6�   $��Bottom=�6�   &   System.Windows.Controls.DataGrid  5   
   -	   5          Name$ layersDataGrid��5         AutoGenerateColumns .  $��17,98,118,62q�6k   ��6+   �� *
Collection+	   Columns	 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Layer #��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 66   �� *layer+  5      $
 Neuron count��6%    6;   �� *neuronCount+  5      $
 Function��6%    67   �� *
function+5      5      5      
   M S B A M L   `   `   `  ����K  GDigitRecognition, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DigitRecognitionDigitRecognition  5   	   V RAccord.Controls, Version=3.8.0.0, Culture=neutral, PublicKeyToken=fa1a88e29555ccf7J6clr-namespace:Accord.Controls;assembly=Accord.ControlsAccord.Controls 5   	   Q MZedGraph, Version=5.1.7.430, Culture=neutral, PublicKeyToken=02a83cbd123fcd605(clr-namespace:ZedGraph;assembly=ZedGraphZedGraph 5   	   !    DigitRecognition.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:DigitRecognition     5   	   KControls6clr-namespace:Accord.Controls;assembly=Accord.Controls     5   	   =ZedGraph(clr-namespace:ZedGraph;assembly=ZedGraph     5   	     � Title$  NeuroNet Analizer��5
   	   $	��459��6#   $	��606��60   .��5      � .+�5   
   �� -   5         ��Name$ 
tabControl��5      $��
10,0,10,10q�6)   .@�5      �� $��TrainTab��6   .��5      � ���	#FFE5E5E5 J� RenderTransformOrigin$ 0.5,0.5&�6.    � RowDefinitions 5      �� 5      $
��159* �6(   �� 5      $
��148* �6(   .+�5      �� -   5         ��Name$ reconfigureButton��5      $��Reconfigure NN��68   $��
0,51,104,0q�6Q   $	��Top=�6e   $��Right=�6}   $	��150��6�   $ 
1.018,3.15&�6�   6�   �� 5      -   5         ��Name$ label��5      $��Learning Rate =��6+   $��Right=�6E   $��0,148,160,0q�6a   $	��Top=�6v   $��2��6�   �� 5      -   5         ��Name$ learningRateTextBox��5      $��19��6;   $��0,152,135,0q�6G    �� TextWrapping$
 Wrap=�6\   $	��0.4��6p   $	��Top=�6{   $��Right=�6�   $��25��6�   �� 5      -   5        $ label1��5      $��Momentum = ��6,   $��Right=�6B   $��0,170,167,0q�6^   $	��Top=�6s   �� 5      -   5        $ momentumTextBox��5      $��18��67   $��0,174,135,0q�6C   $
 Wrap=�6X   $	��0.4��6l   $	��Top=�6w   $��Right=�6�   $��25��6�   �� 5      -   5         ��Name$ comboBox��5      $��
0,126,41,0q�61   $	��Top=�6E   $��Right=�6]   $	��119��6y   .@�5      �� $
��None��6'   5      �� 5      -   5        $ label2��5      $��Optimizer :��6,   $��0,126,186,0q�6B   $	��Top=�6W   $��Right=�6o   $��68��6�   �� 5      -	   5        $ label3��5      $��Net configuration:��6,   $��
0,71,148,0q�6I   $	��Top=�6]   $��Right=�6u   $	��106��6�   �� 5      -
   5        $ label4��5      $��Activation function:��6,   $��
0,97,142,0q�6K   $	��Top=�6_   $��Right=�6w   $	��112��6�   �� 5      -   5        $ configurationLabel��5      $��784 - 30 - 10��68   $��	0,71,24,0q�6P   $	��Top=�6c   $��Right=�6{   $	��113��6�   �� 5      -   5        $ activationFuncLabel��5      $��Sigmoid(0.4)��69   $��	0,97,83,0q�6P   $	��Top=�6c   $��Right=�6{   $��54��6�   �� 5       -   5        $ buttonTrain��5       $��Train��62   $��
0,18,214,0q�6B   $	��Top=�6V   $��Right=�6�   $��40��6�   $��1��6�   7  1System.Windows.Forms.Integration.WindowsFormsHost  5!      -   5        $��windowsFormsHost1��5!   '   $��
0,0,273,11q�6@   $ 0.5,0.5&�6T   	   Child.	 5"        ZedGraph.ZedGraphControl  -   5        errorGraphControl��[ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"  System.Windows.Forms.Control
   Width$	
 400��5"   N      Height$	 400��6Z   5#        5$      -   5        $��windowsFormsHost2��5$   '   $��
0,10,273,0q�6@   $ 0.5,0.5&�6T   $��1��6t   .	 5%        -   5        accGraphControl��$	
 400��5%   L   $	 400��6X   5&      �� 5'      -   5        $ 
buttonLoad��5'      $��Load pretrained��61   $��	0,1,104,0q�6K   $	��Top=�6^   $��Right=�6v   $��92��6�   6�   �� 5(      -   5        $ applyLearningRateButton��5(      $��Ok��6>   $��0,151,104,0q�6K   $	��Top=�6`   $��20��6x   $��Right=�6�   $��26��6�   �� 5)      -   5        $ applyMomentumButton��5)      $��Ok��6:   $��0,173,104,0q�6G   $	��Top=�6\   $��20��6t   $��Right=�6�   $��26��6�   �� 5*      -   5        $ label5��5*      $��	Epochs = ��6,   $��
0,38,195,0q�6@   $��1��6T   $	��Top=�6a   $��Right=�6y   $��62��6�   �� 5+      -   5        $ epochsTextBox��5+      $��19��65   $��
0,42,171,0q�6A   $��1��6U   $
 Wrap=�6b   $��10��6v   $	��Top=�6�   $��Right=�6�   $��25��6�   6�   �� 5,      -   5        $ 
buttonSave��5,      $
��Save��61   $��	0,1,201,0q�6@   $	��Top=�6S   {�.  $��Right=�6}   $��53��6�   6�   �� 5-      -   5        $ 
buttonStop��5-      $
��Stop��61   $��
0,17,171,0q�6@   $	��Top=�6T   {�.  $��Right=�6�   $��40��6�   $��1��6�   $��21��6�   �� 5.      -   5        $ buttonClearGraphs��5.      $��Clear Graphs��68   $��
0,26,165,0q�6O   $	��Top=�6c   $��Right=�6�   $��89��6�   �� 5/      -   5        $ buttonReset��5/      $��Reset��62   $��0,1,24,0q�6B   $	��Top=�6T   $��Right=�6�   $��75��6�   �� 50      -   5        $ label8��50      $��Test acccuracy = ��6,   $��Right=�6H   $��
0,69,154,0q�6d   $��1��6x   $	��Top=�6�   �� 51      -   5        $ buttonShowTestDetails��51      $��Details��6<   $��Right=�6N   $��
0,95,171,0q�6j   $��1��6~   $	��Top=�6�   $��83��6�   6�   �� 52      -   5        $ labelTestAcc��52      $�� ��62   $��Right=�6>   $��	0,69,86,0q�6Z   $��1��6m   $	��Top=�6z   $��63��6�   53      54      �� 55      $��TestTab��6   .��56      � ���	#FFE5E5E5.+�57      �� -   5         ��Name$ imageDigitToRec��57      $
��Left=�65   $	��204��6P   $��	10,10,0,0q�6]   $	��Top=�6p   $	��204��6�   6�   �� 58      -   5        $ buttonPredict��58      $��Predict��64   $
��Left=�6F   $��
10,219,0,0q�6a   $	��Top=�6u   $	��204��6�   6�   �� 59      -    5        $ buttonClear��59      $��X��62   $
��Left=�6>   $��
214,10,0,0q�6Y   $	��Top=�6m   $��21��6�   $ -0.44,0&�6�   6�   �� 5:      -!   5        $ labelAnswer��5:      $��Digit��61   $
��Left=�6A   $��
78,264,0,0q�6\   $	��Top=�6p   �� 5;      -"   5        $ 	labelProb��5;      $
��Prob��6/   $
��Left=�6>   $��
94,288,0,0q�6Y   $	��Top=�6m   $ 	0.5,0.538&�6�   �� 5<      -#   5        $ label6��5<      $��Answer =��6,   $
��Left=�6?   $��
21,264,0,0q�6Z   $	��Top=�6n   �� 5=      -$   5        $ label7��5=      $��Probability =��6,   $
��Left=�6D   $��
21,288,0,0q�6_   $	��Top=�6s   �� 5>      -%   5        $ labelProbVector��5>      $�� ��65   $
��Left=�6A   $��
36,334,0,0q�6\   $	��Top=�6p   $y�Hidden=�6�   �� 5?      -&   5        $ labelBeforeProbVec��5?      $��Probability vector:��68   $
��Left=�6V   $��
21,311,0,0q�6q   $	��Top=�6�   $y�Hidden=�6�   �� 5@      -'   5         ��Name$ checkBox��5@      $��Show probability vector��61   $
��Left=�6S   $��
10,244,0,0q�6n   $	��Top=�6�   6�   5A      5B      5C      5E      5F      
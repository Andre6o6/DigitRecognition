# DigitRecognition
Курсовой проект по теме "Изучение алгоритмов распознавания образов на основе нейронных сетей", написанный на C#/WPF, нейронные сети реализуются с помощью библиотеки Accord.NET.

Он позволяет сконфигурировать нейронную сеть и обучить ее на MNISTе, проанализировать график изменения ошибки и точности на тренировочном и валидационном датасете.

### Интерфейс
В тренировочном окне представлены графики изменения ошибки и точности, а также в нем можно изменить конфигурацию сети, сохранить/загрузить ее, настроить параметры обучения и проанализировать его результаты.

![Train tab](/docs/nna-train.png) 

В тестовом окне можно протестировать сеть на символах, нарисованных собственноручно.

![Test tab](/docs/nna-test.png)

### Setup 
Код был написан и протестирован в Visual Studio 15. 
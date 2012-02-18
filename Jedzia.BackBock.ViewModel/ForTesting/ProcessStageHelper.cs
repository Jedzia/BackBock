// <copyright file="$FileName$" company="$Company$">
// Copyright (c) 2008 All Right Reserved
// </copyright>
// <author>Jedzia</author>
// <email>jed69@gmx.de</email>
// <date>$date$</date>
// <summary>$summary$</summary>
namespace Jedzia.BackBock.ViewModel.ForTesting
{
    using System.Windows;
    using System.Windows.Controls;

    internal enum ProcessStage
    {
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5
    }

    public class ProcessStageHelper : DependencyObject
    {
        #region Fields

        public static readonly DependencyProperty
            ProcessCompletionProperty = DependencyProperty.RegisterAttached(
                "ProcessCompletion",
                typeof(double),
                typeof(ProcessStageHelper),
                new PropertyMetadata(
                    0.0,
                    OnProcessCompletionChanged));

        public static readonly DependencyProperty ProcessStageProperty =
            ProcessStagePropertyKey.DependencyProperty;

        public static readonly DependencyPropertyKey
            ProcessStagePropertyKey =
                DependencyProperty.RegisterAttachedReadOnly(
                    "ProcessStage",
                    typeof(ProcessStage),
                    typeof(ProcessStageHelper),
                    new PropertyMetadata(ProcessStage.Stage1));

        #endregion

        private static void OnProcessCompletionChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var progress = (double)e.NewValue;
            var bar = ((FrameworkElement)d).TemplatedParent
                      as ProgressBar;
            if (bar == null)
            {
                return;
            }
            if (progress >= 0 && progress < 20)
            {
                bar.SetValue(ProcessStagePropertyKey, ProcessStage.Stage1);
            }
            if (progress >= 20 && progress < 40)
            {
                bar.SetValue(ProcessStagePropertyKey, ProcessStage.Stage2);
            }
            if (progress >= 40 && progress < 60)
            {
                bar.SetValue(ProcessStagePropertyKey, ProcessStage.Stage3);
            }
            if (progress >= 60 && progress < 80)
            {
                bar.SetValue(ProcessStagePropertyKey, ProcessStage.Stage4);
            }
            if (progress >= 80 && progress <= 100)
            {
                bar.SetValue(ProcessStagePropertyKey, ProcessStage.Stage5);
            }
        }
    }
}
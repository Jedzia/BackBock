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

    /// <summary>
    /// Represents the actual value of a <see cref="ProcessStageHelper"/> object from Stage1 to Stage5.
    /// </summary>
    internal enum ProcessStage
    {
        /// <summary>
        /// Current range from 0 to 19%.
        /// </summary>
        Stage1,
        
        /// <summary>
        /// Current range from 20 to 39%.
        /// </summary>
        Stage2,
        
        /// <summary>
        /// Current range from 40 to 59%.
        /// </summary>
        Stage3,
        
        /// <summary>
        /// Current range from 60 to 79%.
        /// </summary>
        Stage4,
        
        /// <summary>
        /// Current range from 80 to 100%.
        /// </summary>
        Stage5
    }

    /// <summary>
    /// ProgressBar staging helper.
    /// </summary>
    public class ProcessStageHelper : DependencyObject
    {
        #region Fields

        /// <summary>
        /// Current value of progress.
        /// </summary>
        public static readonly DependencyProperty
            ProcessCompletionProperty = DependencyProperty.RegisterAttached(
                "ProcessCompletion",
                typeof(double),
                typeof(ProcessStageHelper),
                new PropertyMetadata(
                    0.0,
                    OnProcessCompletionChanged));

        /// <summary>
        /// Key of the <see cref="ProcessStagePropertyKey"/> value.
        /// </summary>
        public static readonly DependencyProperty ProcessStageProperty =
            ProcessStagePropertyKey.DependencyProperty;
        
        /// <summary>
        /// Current <see cref="ProcessStage"/> attached dependency property.
        /// </summary>
        public static readonly DependencyPropertyKey
            ProcessStagePropertyKey =
                DependencyProperty.RegisterAttachedReadOnly(
                    "ProcessStage",
                    typeof(ProcessStage),
                    typeof(ProcessStageHelper),
                    new PropertyMetadata(ProcessStage.Stage1));

        #endregion

        /// <summary>
        /// Called when the progress changed.
        /// </summary>
        /// <param name="d">The associated dependency object.</param>
        /// <param name="e">The <see cref="System.Windows.DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
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
namespace Jedzia.BackBock.ViewModel.Util
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Media;

    /// <summary>
    /// Helper methods for visual <see cref="FrameworkElement"/> tree's.
    /// </summary>
    public static class VisualHelper
    {
        /// <summary>
        /// Finds the visual child.
        /// </summary>
        /// <param name="myVisual">The visual root.</param>
        /// <param name="tagOrNameToSearch">The tag or name to search.</param>
        /// <returns>The found <see cref="FrameworkElement"/> or null, if nothing is found.</returns>
        public static FrameworkElement FindVisualChild(Visual myVisual, string tagOrNameToSearch)
        {
            // Enumerate all the descendants of the visual object. Todo: move to Util
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(myVisual); i++)
            {
                // Retrieve child visual at specified index value.
                Visual childVisual = (Visual)VisualTreeHelper.GetChild(myVisual, i);
                FrameworkElement ui = childVisual as FrameworkElement;
                if (ui != null)
                {
                    if ((ui.Tag as string) == tagOrNameToSearch || ui.Name == tagOrNameToSearch)
                    {
                        return ui;
                    }
                }
                // Do processing of the child visual object.

                // Enumerate children of the child visual object.
                var result = FindVisualChild(childVisual, tagOrNameToSearch);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        /// Clone only Plane.
        /// </summary>
        /// <typeparam name="T">Type of the object to clone.</typeparam>
        /// <param name="source">The source object to clone.</param>
        /// <returns>A clone of the source object.</returns>
        public static T CloneXX<T>(this T source) where T : DependencyObject
        {
            Type t = source.GetType();
            T no = (T)Activator.CreateInstance(t);

            Type wt = t;
            while (wt.BaseType != typeof(DependencyObject))
            {
                FieldInfo[] fi = wt.GetFields(BindingFlags.Static | BindingFlags.Public);
                for (int i = 0; i < fi.Length; i++)
                {
                    {
                        DependencyProperty dp = fi[i].GetValue(source) as DependencyProperty;
                        if (dp != null && fi[i].Name != "NameProperty")
                        {
                            DependencyObject obj = source.GetValue(dp) as DependencyObject;
                            if (obj != null)
                            {
                                object o = obj.CloneXX();
                                no.SetValue(dp, o);
                            }
                            else
                            {
                                if (fi[i].Name != "CountProperty" &&
                                    fi[i].Name != "GeometryTransformProperty" &&
                                    fi[i].Name != "ActualWidthProperty" &&
                                    fi[i].Name != "ActualHeightProperty" &&
                                    fi[i].Name != "MaxWidthProperty" &&
                                    fi[i].Name != "MaxHeightProperty" &&
                                    fi[i].Name != "StyleProperty")
                                {
                                    try
                                    {
                                        no.SetValue(dp, source.GetValue(dp));
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }

                            }
                        }
                    }
                }
                wt = wt.BaseType;
            }

            PropertyInfo[] pis = t.GetProperties();
            for (int i = 0; i < pis.Length; i++)
            {

                if (
                    pis[i].Name != "Name" &&
                    pis[i].Name != "Parent" &&
                    pis[i].CanRead && pis[i].CanWrite &&
                    !pis[i].PropertyType.IsArray &&
                    !pis[i].PropertyType.IsSubclassOf(typeof(DependencyObject)) &&
                    pis[i].GetIndexParameters().Length == 0 &&
                    pis[i].GetValue(source, null) != null &&
                    pis[i].GetValue(source, null) == (object)default(int) &&
                    pis[i].GetValue(source, null) == (object)default(double) &&
                    pis[i].GetValue(source, null) == (object)default(float)
                    )
                    pis[i].SetValue(no, pis[i].GetValue(source, null), null);
                else if (pis[i].PropertyType.GetInterface("IList", true) != null)
                {
                    int cnt = (int)pis[i].PropertyType.InvokeMember("get_Count", BindingFlags.InvokeMethod, null, pis[i].GetValue(source, null), null);
                    for (int c = 0; c < cnt; c++)
                    {
                        object val = pis[i].PropertyType.InvokeMember("get_Item", BindingFlags.InvokeMethod, null, pis[i].GetValue(source, null), new object[] { c });

                        object nVal = val;
                        DependencyObject v = val as DependencyObject;
                        if (v != null)
                            nVal = v.CloneXX();

                        pis[i].PropertyType.InvokeMember("Add", BindingFlags.InvokeMethod, null, pis[i].GetValue(no, null), new object[] { nVal });
                    }
                }
            }
            return no;
        }


        /// <summary>
        /// Clones the specified reference element.
        /// </summary>
        /// <param name="referenceElement">The reference element.</param>
        /// <param name="shareRenderingData">if set to <c>true</c> [share rendering data].</param>
        /// <returns></returns>
        /// <remarks><![CDATA[
        /// http://social.msdn.microsoft.com/Forums/en/wpf/thread/209597f0-0e8b-4fbb-a69d-a6479ed96187
        /// https://skydrive.live.com/?cid=fd9a0f1f8dd06954&id=FD9A0F1F8DD06954!2100
        /// ]]>
        /// </remarks>
        public static FrameworkElement Clone(this FrameworkElement referenceElement, bool shareRenderingData)
        {
            return new InternalElement(referenceElement, shareRenderingData);
        }

        /// <summary>
        /// Clone helper object.
        /// </summary>
        internal class InternalElement : FrameworkElement
        {
            private Visual rootVisual;
            private bool shareRenderingData;

            /// <summary>
            /// Initializes a new instance of the <see cref="InternalElement"/> class.
            /// </summary>
            /// <param name="rootVisual">The root visual.</param>
            /// <param name="shareRenderingData">if set to <c>true</c> [shares rendering data].</param>
            public InternalElement(Visual rootVisual, bool shareRenderingData)
            {
                this.rootVisual = rootVisual;
                this.shareRenderingData = shareRenderingData;
            }

            /// <summary>
            /// When overridden in a derived class, measures the size in layout required for child elements and determines a size for the <see cref="T:System.Windows.FrameworkElement"/>-derived class.
            /// </summary>
            /// <param name="availableSize">The available size that this element can give to child elements. Infinity can be specified as a value to indicate that the element will size to whatever content is available.</param>
            /// <returns>
            /// The size that this element determines it needs during layout, based on its calculations of child element sizes.
            /// </returns>
            protected override Size MeasureOverride(Size availableSize)
            {
                var fe = rootVisual as FrameworkElement;
                if (fe != null && !fe.IsLoaded)
                {
                    fe.ApplyTemplate();
                }

                UIElement element = rootVisual as UIElement;
                if (element != null && element.IsMeasureValid)
                {
                    return element.DesiredSize;
                }

                return base.MeasureOverride(availableSize);
            }

            /// <summary>
            /// When overridden in a derived class, positions child elements and determines a size for a <see cref="T:System.Windows.FrameworkElement"/> derived class.
            /// </summary>
            /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
            /// <returns>
            /// The actual size used.
            /// </returns>
            protected override Size ArrangeOverride(Size finalSize)
            {
                var fe = rootVisual as FrameworkElement;
                if (fe != null && fe.IsArrangeValid)
                {
                    return new Size(fe.ActualWidth, fe.ActualHeight);
                }

                UIElement element = rootVisual as UIElement;
                if (element != null && element.IsArrangeValid)
                {
                    return element.RenderSize;
                }

                return base.ArrangeOverride(finalSize);
            }

            /// <summary>
            /// When overridden in a derived class, participates in rendering operations that are directed by the layout system. The rendering instructions for this element are not used directly when this method is invoked, and are instead preserved for later asynchronous use by layout and drawing.
            /// </summary>
            /// <param name="drawingContext">The drawing instructions for a specific element. This context is provided to the layout system.</param>
            protected override void OnRender(DrawingContext drawingContext)
            {
                if (rootVisual != null)
                {
                    SendToContext(rootVisual, rootVisual, drawingContext, shareRenderingData);
                }
            }

            /// <summary>
            /// Ifs the share freezable.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="freezable">The freezable.</param>
            /// <param name="shareRenderingData">if set to <c>true</c> [share rendering data].</param>
            /// <returns></returns>
            private static T IfShareFreezable<T>(T freezable, bool shareRenderingData) where T : Freezable
            {
                T result = freezable;
                if (result != null && !shareRenderingData)
                {
                    result = (T)freezable.CloneCurrentValue();
                    if (freezable.CanFreeze)
                    {
                        result.Freeze();
                    }
                }
                return result;
            }

            private static void SendToContext(Visual parent, Visual child, DrawingContext drawingContext, bool shareRenderingData)
            {
                var drawing = IfShareFreezable(VisualTreeHelper.GetDrawing(child), shareRenderingData);
                var transform = child.TransformToAncestor(parent) as Transform;
                var effect = IfShareFreezable(VisualTreeHelper.GetBitmapEffect(child), shareRenderingData);
                var clip = IfShareFreezable(VisualTreeHelper.GetClip(child), shareRenderingData);
                var opacity = VisualTreeHelper.GetOpacity(child);
                var opacityMask = IfShareFreezable(VisualTreeHelper.GetOpacityMask(child), shareRenderingData);
                var guidelinesX = IfShareFreezable(VisualTreeHelper.GetXSnappingGuidelines(child), shareRenderingData);
                var guidelinesY = IfShareFreezable(VisualTreeHelper.GetYSnappingGuidelines(child), shareRenderingData);
                var pushCounter = 0;

                if (transform != null && transform != Transform.Identity)
                {
                    drawingContext.PushTransform(transform);
                    pushCounter++;
                }

                if (effect != null)
                {
                    drawingContext.PushEffect(effect, null);
                    pushCounter++;
                }

                if (clip != null)
                {
                    drawingContext.PushClip(clip);
                    pushCounter++;
                }

                if (opacity >= 0 && (int)opacity < 1)
                {
                    drawingContext.PushOpacity(opacity);
                    pushCounter++;
                }

                if (opacityMask != null)
                {
                    drawingContext.PushOpacityMask(opacityMask);
                    pushCounter++;
                }

                if (guidelinesX != null && guidelinesY != null)
                {
                    drawingContext.PushGuidelineSet(new GuidelineSet()
                    {
                        GuidelinesX = guidelinesX,
                        GuidelinesY = guidelinesY
                    });
                    pushCounter++;
                }

                if (drawing != null) drawingContext.DrawDrawing(drawing);

                var count = VisualTreeHelper.GetChildrenCount(child);
                for (int i = 0; i < count; i++)
                {
                    Visual visual = VisualTreeHelper.GetChild(child, i) as Visual;
                    if (visual != null)
                    {
                        SendToContext(child, visual, drawingContext, shareRenderingData);
                    }
                }

                while (pushCounter-- > 0)
                {
                    drawingContext.Pop();
                }
            }
        }
    }
}
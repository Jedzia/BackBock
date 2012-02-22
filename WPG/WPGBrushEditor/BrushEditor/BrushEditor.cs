﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using System.Reflection;
using System.Windows;

namespace ICSharpCode.WpfDesign.Designer.PropertyGrid.Editors.BrushEditor
{
	public class BrushEditor : INotifyPropertyChanged
	{
		public BrushEditor()
		{
			GradientStopCollection stops = new GradientStopCollection();
			stops.Add(new GradientStop(Colors.Black, 0));
			stops.Add(new GradientStop(Colors.White, 1));

			linearGradientBrush = new LinearGradientBrush(stops);
			linearGradientBrush.EndPoint = new Point(1, 0);
			radialGradientBrush = new RadialGradientBrush(stops);
		}

		public static BrushItem[] SystemBrushes = typeof(SystemColors)
			.GetProperties(BindingFlags.Static | BindingFlags.Public)
			.Where(p => p.PropertyType == typeof(SolidColorBrush))
			.Select(p => new BrushItem() { Name = p.Name, Brush = (Brush)p.GetValue(null, null) })
			.ToArray();

		public static BrushItem[] SystemColors = typeof(SystemColors)
			.GetProperties(BindingFlags.Static | BindingFlags.Public)
			.Where(p => p.PropertyType == typeof(Color))
			.Select(p => new BrushItem()
			{
        		Name = p.Name,
        		Brush = new SolidColorBrush((Color)p.GetValue(null, null))
			})
			.ToArray();

		SolidColorBrush solidColorBrush = new SolidColorBrush(Colors.White);
		LinearGradientBrush linearGradientBrush;
		RadialGradientBrush radialGradientBrush;

       

        PropertyNode property;

        public PropertyNode Property
        {
			get {
				return property;
			}
			set {
				property = value;
				/*if (property != null) {
					var f = property.Value as Freezable;
					if (f != null && f.IsFrozen) property.Value = f.Clone();
				}*/
				DetermineCurrentKind();
				RaisePropertyChanged("Property");
				RaisePropertyChanged("Brush");
			}
		}

		public Brush Brush {
			get {
				if (property != null) {
					return property.Value as Brush;
				}
				return null;
			}
			set {
				if (property != null && property.Value != value) {
					property.Value = value;
					DetermineCurrentKind();
					RaisePropertyChanged("Brush");
				}
			}
		}

		void DetermineCurrentKind()
		{
			if (Brush == null) {
				CurrentKind = BrushEditorKind.None;
			}
			else if (Brush is SolidColorBrush) {
				solidColorBrush = Brush as SolidColorBrush;
				CurrentKind = BrushEditorKind.Solid;
			}
			else if (Brush is LinearGradientBrush) {
				linearGradientBrush = Brush as LinearGradientBrush;
				radialGradientBrush.GradientStops = linearGradientBrush.GradientStops;
				CurrentKind = BrushEditorKind.Linear;
			}
			else if (Brush is RadialGradientBrush) {
				radialGradientBrush = Brush as RadialGradientBrush;
				linearGradientBrush.GradientStops = linearGradientBrush.GradientStops;
				CurrentKind = BrushEditorKind.Radial;
			}
		}

		BrushEditorKind currentKind;

		public BrushEditorKind CurrentKind {
			get {
				return currentKind;
			}
			set {
				currentKind = value;
				RaisePropertyChanged("CurrentKind");

				switch (CurrentKind) {
					case BrushEditorKind.None:
						Brush = null;
						break;

					case BrushEditorKind.Solid:
						Brush = solidColorBrush;
						break;

					case BrushEditorKind.Linear:
						Brush = linearGradientBrush;
						break;

					case BrushEditorKind.Radial:
						Brush = radialGradientBrush;
						break;

					case BrushEditorKind.List:
						Brush = solidColorBrush;
						break;
				}
			}
		}

		public double GradientAngle {
			get {
				var x = linearGradientBrush.EndPoint.X - linearGradientBrush.StartPoint.X;
				var y = linearGradientBrush.EndPoint.Y - linearGradientBrush.StartPoint.Y;
				return Vector.AngleBetween(new Vector(1, 0), new Vector(x, -y));
			}
			set {
				var d = value * Math.PI / 180;
				var p = new Point(Math.Cos(d), -Math.Sin(d));
				var k = 1 / Math.Max(Math.Abs(p.X), Math.Abs(p.Y));
				p.X *= k;
				p.Y *= k;
				var p2 = new Point(-p.X, -p.Y);
				linearGradientBrush.StartPoint = new Point((p2.X + 1) / 2, (p2.Y + 1) / 2);
				linearGradientBrush.EndPoint = new Point((p.X + 1) / 2, (p.Y + 1) / 2);
				RaisePropertyChanged("GradientAngle");
			}
		}

		public IEnumerable<BrushItem> AvailableColors {
			get { return SystemColors; }
		}

		public IEnumerable<BrushItem> AvailableBrushes {
			get { return SystemBrushes; }
		}

		public void MakeGradientHorizontal()
		{
			GradientAngle = 0;
		}

		public void MakeGradientVertical()
		{
			GradientAngle = -90;
		}

		public void Commit()
		{
			if (Brush != null) {
				Property.Value = Brush.Clone();
			}
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		void RaisePropertyChanged(string name)
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		#endregion
	}

	public enum BrushEditorKind
	{
		None,
		Solid,
		Linear,
		Radial,
		List
	}

	public class BrushItem
	{
		public string Name { get; set; }
		public Brush Brush { get; set; }
	}
}

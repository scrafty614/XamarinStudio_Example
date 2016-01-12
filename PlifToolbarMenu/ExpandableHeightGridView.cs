﻿using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Views;

namespace PlifToolbarMenu
{
	public class ExpandableHeightGridView: GridView
	{
		bool _isExpanded = false;

		public ExpandableHeightGridView(Context context) : base(context)
		{            
		}

		public ExpandableHeightGridView(Context context, IAttributeSet attrs) : base(context, attrs)
		{            
		}

		public ExpandableHeightGridView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{            
		}

		public bool IsExpanded
		{
			get { return _isExpanded; }

			set { _isExpanded = value;  }
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			// HACK! TAKE THAT ANDROID!
			if (IsExpanded)
			{
				// Calculate entire height by providing a very large height hint.
				// View.MEASURED_SIZE_MASK represents the largest height possible.
				int expandSpec = MeasureSpec.MakeMeasureSpec( View.MeasuredSizeMask, MeasureSpecMode.AtMost);
				base.OnMeasure(widthMeasureSpec,expandSpec);                

				var layoutParameters = this.LayoutParameters;
				layoutParameters.Height = this.MeasuredHeight;
			}
			else
			{
				base.OnMeasure(widthMeasureSpec,heightMeasureSpec);    
			}
		}
	}
}


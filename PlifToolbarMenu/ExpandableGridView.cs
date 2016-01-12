using System;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Views;

namespace PlifToolbarMenu
{
	public class ExpandableGridView : GridView
	{	

	
		bool _isExpanded = false;

		public ExpandableGridView(Context context) : base(context)
		{
			
			//super(context);
		}

		public ExpandableGridView(Context context, IAttributeSet attrs) : base (context, attrs)
		{
			//super(context, attrs);
		}

		public ExpandableGridView(Context context,  IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			//super(context, attrs, defStyle);
		}

		public ExpandableGridView(Context context,  IAttributeSet attrs, int defStyle, int defStyleRes) : base(context, attrs, defStyle, defStyleRes)
		{
			//super(context, attrs, defStyle);
		}

		bool expanded = false;

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

		public void setExpanded(bool expanded)
		{
			this.expanded = expanded;
		}
	}
}


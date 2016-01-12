﻿using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;
namespace PlifToolbarMenu
{
	public class CircleImageView : ImageView
	{	
		private int borderWidth;
		private int canvasSize;
		private Bitmap image;
		private Paint paint;
		private Paint paintBorder;
		private static ScaleType escala = ScaleType.CenterCrop;



		public CircleImageView(Context context)	: this(context, null)
		{

			base.SetScaleType (escala);
			Log.Debug ("ImageCircle","Si se inició!!!");
		}

		public CircleImageView(Context context, IAttributeSet attrs) : this(context, attrs, Resource.Attribute.circularImageViewStyle)
		{
		}

		public override ScaleType GetScaleType(){
			return escala;
		}

		public override void SetScaleType(ScaleType scale){
			if (scale != escala) {
				throw new AndroidException ("Chingado! Que no le muevas a la escala");
			}

		}



		public CircleImageView(Context context, IAttributeSet attrs, int defStyle)
			: base(context, attrs, defStyle)
		{

			// init paint
			paint = new Paint();
			paint.AntiAlias = true;

			paintBorder = new Paint();
			paintBorder.AntiAlias = true;

			// load the styled attributes and set their properties
			TypedArray attributes = context.ObtainStyledAttributes(attrs, Resource.Styleable.CircularImageView, defStyle, 0);

			if (attributes.GetBoolean(Resource.Styleable.CircularImageView_border, true))
			{
				int defaultBorderSize = (int)(4 * context.Resources.DisplayMetrics.Density+ 0.5f);
				BorderWidth = attributes.GetDimensionPixelOffset(Resource.Styleable.CircularImageView_border_width, defaultBorderSize);
				BorderColor = attributes.GetColor(Resource.Styleable.CircularImageView_border_color, Color.White);
			}

			if (attributes.GetBoolean(Resource.Styleable.CircularImageView_shadow, false))
			{
				addShadow();
			}
		}
		public void addShadow()
		{
			SetLayerType(LayerType.Software, paintBorder);
			paintBorder.SetShadowLayer(4.0f, 0.0f, 2.0f, Color.Gray);
		}
		public virtual int BorderWidth
		{
			set
			{
				this.borderWidth = 0;
				this.RequestLayout();
				this.Invalidate();
			}
		}

		public virtual int BorderColor
		{
			set
			{
				if (paintBorder != null)
				{
					paintBorder.Color = Color.Transparent;
				}
				this.Invalidate();
			}
		}
		protected override void OnDraw(Canvas canvas)
		{
			// load the bitmap
			image = drawableToBitmap(Drawable);

			// init shader
			if (image != null)
			{

				canvasSize = canvas.Width;
				if (canvas.Height < canvasSize)
				{
					canvasSize = canvas.Height;
				}

				BitmapShader shader = new BitmapShader(Bitmap.CreateScaledBitmap(image, canvasSize, canvasSize, false), Shader.TileMode.Clamp, Shader.TileMode.Clamp);
				paint.SetShader(shader);

				int circleCenter = (canvasSize - (borderWidth * 2)) / 2;
				canvas.DrawCircle(circleCenter + borderWidth, circleCenter + borderWidth, ((canvasSize - (borderWidth * 2)) / 2) + borderWidth - 4.0f, paintBorder);
				canvas.DrawCircle(circleCenter + borderWidth, circleCenter + borderWidth, ((canvasSize - (borderWidth * 2)) / 2) - 4.0f, paint);
			}
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			int width = measureWidth(widthMeasureSpec);
			int height = measureHeight(heightMeasureSpec);
			SetMeasuredDimension(width, height);
		}

		private int measureWidth(int measureSpec)
		{
			int result = 0;
			var specMode = MeasureSpec.GetMode(measureSpec);
			var specSize = MeasureSpec.GetSize(measureSpec);

			if (specMode == MeasureSpecMode.Exactly)
			{
				// The parent has determined an exact size for the child.
				result = specSize;
			}
			else if (specMode == MeasureSpecMode.AtMost)
			{
				// The child can be as large as it wants up to the specified size.
				result = specSize;
			}
			else
			{
				// The parent has not imposed any constraint on the child.
				result = canvasSize;
			}

			return result;
		}

		private int measureHeight(int measureSpecHeight)
		{
			int result = 0;
			var specMode = MeasureSpec.GetMode(measureSpecHeight);
			int specSize = MeasureSpec.GetSize(measureSpecHeight);

			if (specMode == MeasureSpecMode.Exactly)
			{
				// We were told how big to be
				result = specSize;
			}
			else if (specMode == MeasureSpecMode.AtMost)
			{
				// The child can be as large as it wants up to the specified size.
				result = specSize;
			}
			else
			{
				// Measure the text (beware: ascent is a negative number)
				result = canvasSize;
			}

			return (result + 2);
		}

		public virtual Bitmap drawableToBitmap(Drawable drawable)
		{
			if (drawable == null)
			{
				return null;
			}
			else if (drawable is BitmapDrawable)
			{
				return ((BitmapDrawable)drawable).Bitmap;
			}

			Bitmap bitmap = Bitmap.CreateBitmap(drawable.IntrinsicWidth, drawable.IntrinsicHeight, Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas(bitmap);
			drawable.SetBounds(0, 0, canvas.Width, canvas.Height);
			drawable.Draw(canvas);

			return bitmap;
		}
	}
}


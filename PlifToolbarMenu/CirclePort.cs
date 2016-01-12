
using System;
using Android.Content;
//using Android.content.Context;
using Android.Content.Res;
//using Android.content.res.TypedArray;
using Android.Graphics;
/*using Android.Graphics.Bitmap;
using Android.Graphics.BitmapShader;
using Android.Graphics.Canvas;
using Android.Graphics.Color;
using Android.Graphics.ColorFilter;
using Android.Graphics.Matrix;
using Android.Graphics.Paint;
using Android.Graphics.RectF;
using Android.Graphics.Shader;
*/
using Android.Graphics.Drawables;
/*
using Android.Graphics.Drawable.BitmapDrawable;
using Android.Graphics.Drawable.ColorDrawable;
using Android.Graphics.Drawable.Drawable;
*/
/*
using Android.net.Uri;
using Android.Supports;
using Android.support.annotation.ColorRes;
//using Android.support.annotation.DrawableRes;
*/
using Android.Util;
using Android.Widget;

using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Runtime;


namespace PlifToolbarMenu
{

	public class CirclePort //: ImageView 
	{/*

		private static ScaleType SCALE_TYPE = ScaleType.CenterCrop;

		private static Bitmap.Config BITMAP_CONFIG = Bitmap.Config.Argb8888;
		private static int COLORDRAWABLE_DIMENSION = 2;

		private static int DEFAULT_BORDER_WIDTH = 0;
		private static int DEFAULT_BORDER_COLOR = Color.Black;
		private static int DEFAULT_FILL_COLOR = Color.Transparent;
		private static bool DEFAULT_BORDER_OVERLAY = false;

		private RectF mDrawableRect = new RectF();
		private RectF mBorderRect = new RectF();

		private Matrix mShaderMatrix = new Matrix();
		private Paint mBitmapPaint = new Paint();
		private Paint mBorderPaint = new Paint();
		private Paint mFillPaint = new Paint();

		private Color mBorderColor = Color.Black; //DEFAULT_BORDER_COLOR;
		private int mBorderWidth = 0; // DEFAULT_BORDER_WIDTH;
		private Color mFillColor = Color.Transparent; //DEFAULT_FILL_COLOR;

		private Bitmap mBitmap;
		private BitmapShader mBitmapShader;
		private int mBitmapWidth;
		private int mBitmapHeight;

		private float mDrawableRadius;
		private float mBorderRadius;

		private ColorFilter mColorFilter;

		private bool mReady;
		private bool mSetupPending;
		private bool mBorderOverlay;

		private Bitmap image;
		private Paint paint;
		private Paint paintBorder;

		private int canvasSize;
		private int borderWidth;

		public CirclePort(Context context) : this(context, null) {
			//super(context);

			init();
		}

		public CirclePort(Context context, IAttributeSet attrs): this(context, attrs, Resource.Attribute.circularImageViewStyle) {
			//this(context, attrs, 0);
		}

		public CirclePort(Context context, IAttributeSet attrs, int defStyle): base(context, attrs, defStyle){
			//super(context, attrs, defStyle);

			//TypedArray a = context.obtainStyledAttributes(attrs, R.styleable.CircleImageView, defStyle, 0);
			TypedArray a = context.ObtainStyledAttributes(attrs, Resource.Styleable.CircularImageView, defStyle, 0);

			//mBorderWidth = a.getDimensionPixelSize(R.styleable.CircleImageView_civ_border_width, DEFAULT_BORDER_WIDTH);
			mBorderWidth = a.GetDimensionPixelSize (Resource.Styleable.CircleImageView_civ_border_width, DEFAULT_BORDER_WIDTH);

			//mBorderColor = a.getColor(R.styleable.CircleImageView_civ_border_color, DEFAULT_BORDER_COLOR);
			mBorderColor = a.GetColor(Resource.Styleable.CircleImageView_civ_border_color, DEFAULT_BORDER_COLOR);
			//mBorderOverlay = a.getBoolean(R.styleable.CircleImageView_civ_border_overlay, DEFAULT_BORDER_OVERLAY);
			mBorderOverlay = a.GetBoolean(Resource.Styleable.CircleImageView_civ_border_overlay, DEFAULT_BORDER_OVERLAY);
			//mFillColor = a.getColor(R.styleable.CircleImageView_civ_fill_color, DEFAULT_FILL_COLOR);
			mFillColor = a.GetColor(Resource.Styleable.CircleImageView_civ_fill_color, DEFAULT_FILL_COLOR);
			//a.recycle();
			a.Recycle();

			init();
		}

		private void init() {
			base.SetScaleType (SCALE_TYPE);
			//super.setScaleType(SCALE_TYPE);
			mReady = true;

			if (mSetupPending) {
				setup();
				mSetupPending = false;
			}
		}



		public ScaleType getScaleType() {
			return SCALE_TYPE;
		}


		public override void SetScaleType(ScaleType scaleType) {
			if (scaleType != SCALE_TYPE) {
				//throw new IllegalArgumentException(String.format("ScaleType %s not supported.", scaleType));
				//aqui vamos a lanzar una excepcion
			}
		}


		public override void SetAdjustViewBounds(bool adjustViewBounds) {
			if (adjustViewBounds) {
				//throw new IllegalArgumentException("adjustViewBounds not supported.");
				//hay que lanzar otra excepcion aqui
			}
		}


		protected override void OnDraw(Canvas canvas) {

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

			/*
			if (mBitmap == null) {
				return;
			}

			if (mFillColor != Color.Transparent) {/*
				//canvas.drawCircle(getWidth() / 2.0f, getHeight() / 2.0f, mDrawableRadius, mFillPaint);
				canvas.DrawCircle(getWidth() / 2.0f, getHeight() / 2.0f, mDrawableRadius, mFillPaint);
				canvas.DrawCircle(
					
			}
			//canvas.drawCircle(getWidth() / 2.0f, getHeight() / 2.0f, mDrawableRadius, mBitmapPaint);
			canvas.DrawCircle(getWidth() / 2.0f, getHeight() / 2.0f, mDrawableRadius, mBitmapPaint);
			if (mBorderWidth != 0) {
				//canvas.drawCircle(getWidth() / 2.0f, getHeight() / 2.0f, mBorderRadius, mBorderPaint);
				canvas.DrawCircle(getWidth() / 2.0f, getHeight() / 2.0f, mDrawableRadius, mBorderPaint);
			}
				*//*
		}


		protected override void OnSizeChanged(int w, int h, int oldw, int oldh) {
			//super.onSizeChanged(w, h, oldw, oldh);
			base.OnSizeChanged(w, h, oldw, oldh);
			setup();
		}

		public int getBorderColor() {
			return mBorderColor;
		}

		public void setBorderColor(int borderColor) {
					/*
			if (borderColor == mBorderColor) {
				return;
			}

			mBorderColor = borderColor;
			mBorderPaint.setColor(mBorderColor);
					mBorderPaint.
			invalidate();
			*//*
		}

		public void setBorderColorResource(int borderColorRes) {
			//setBorderColor(getContext().getResources().getColor(borderColorRes));
			setBorderColor (Context.Resources.GetColor (borderColorRes));
		}

		public int getFillColor() {
			return mFillColor;
		}

		public void setFillColor(int fillColor) {/*
			if (fillColor == mFillColor) {
				return;
			}

			mFillColor = fillColor;
			mFillPaint.setColor(fillColor);
			
			invalidate();*//*
		}

		public int getBorderWidth() {
			return mBorderWidth;
		}

		public void setBorderWidth(int borderWidth) {
			if (borderWidth == mBorderWidth) {
				return;
			}

			mBorderWidth = borderWidth;
			setup();
		}

		public bool isBorderOverlay() {
			return mBorderOverlay;
		}

		public void setBorderOverlay(bool borderOverlay) {
			if (borderOverlay == mBorderOverlay) {
				return;
			}

			mBorderOverlay = borderOverlay;
			setup();
		}


		public override void SetImageBitmap(Bitmap bm) {
			//super.setImageBitmap(bm);
			base.SetImageBitmap (bm);
			mBitmap = bm;
			setup();
		}


		public override void SetImageDrawable(Drawable drawable) {
			base.SetImageDrawable (drawable);
			//super.setImageDrawable(drawable);
			//mBitmap = getBitmapFromDrawable(drawable);
					mBitmap = drawableToBitmap (drawable);
			setup();
		}


				public override void SetImageResource(int resId)
				{
					base.SetImageResource(resId);
					//this.Initialize();
				}


				public void SetImageURI(Android.Net.Uri uri){
					base.SetImageURI (uri);
		}

	/*

		public override void SetImageURI(Uri uri) {
			//super.setImageURI(uri);
			base.SetImageURI (uri);
			setup();
		}
*//*

		public override void SetColorFilter(ColorFilter cf) {
			if (cf == mColorFilter) {
				return;
			}

			mColorFilter = cf;
			//mBitmapPaint.setColorFilter(mColorFilter);
			mBitmapPaint.SetColorFilter (mColorFilter);

			Invalidate();
		}

		

		private void setup() {
			if (!mReady) {
				mSetupPending = true;
				return;
			}

					if (mBitmap.Width == 0 && mBitmap.Height == 0) {
				return;
			}

			if (mBitmap == null) {
				Invalidate();
				return;
			}

			//mBitmapShader = new BitmapShader(mBitmap, Shader.TileMode.CLAMP, Shader.TileMode.CLAMP);
			mBitmapShader = new BitmapShader (mBitmap, Shader.TileMode.Clamp, Shader.TileMode.Clamp);

			//mBitmapPaint.setAntiAlias(true);
					mBitmapPaint.AntiAlias=true;
			//mBitmapPaint.setShader(mBitmapShader);
					mBitmapPaint.Shader = mBitmapShader;
				

			//mBorderPaint.setStyle(Paint.Style.STROKE);
			mBorderPaint.SetStyle (Paint.Style.Stroke);
			mBorderPaint.AntiAlias=true;
			//mBorderPaint.setColor(mBorderColor);
					mBorderPaint.Color = mBorderColor;

			//mBorderPaint.setStrokeWidth(mBorderWidth);
					mBorderPaint.StrokeWidth=mBorderWidth;

			mFillPaint.SetStyle(Paint.Style.Fill);
			mFillPaint.AntiAlias=true;
			mFillPaint.Color=mFillColor;

					mBitmapHeight = mBitmap.Height;
					mBitmapWidth = mBitmap.Width;

			//mBorderRect.set(0, 0, getWidth(), getHeight());
			mBorderRect.Set(0,0,mBitmapWidth,mBitmapHeight);
					mBorderRadius = Math.Min((mBorderRect.Height - mBorderWidth) / 2.0f, (mBorderRect.Width - mBorderWidth) / 2.0f);



			//mDrawableRect.Set(mBorderRect);
			mDrawableRect.Set(mBorderRect);
			if (!mBorderOverlay) {
				mDrawableRect.Inset(mBorderWidth, mBorderWidth);

			}
					mDrawableRadius = Math.Min(mDrawableRect.Height / 2.0f, mDrawableRect.Width / 2.0f);

			updateShaderMatrix();
			Invalidate();
		}

		private void updateShaderMatrix() {
			float scale;
			float dx = 0;
			float dy = 0;

			mShaderMatrix.Set(null);


					if (mBitmapWidth * mDrawableRect.Height > mDrawableRect.Width * mBitmapHeight) {
						scale = mDrawableRect.Height / (float) mBitmapHeight;
						dx = (mDrawableRect.Width - mBitmapWidth * scale) * 0.5f;
					} else {
						scale = mDrawableRect.Width / (float) mBitmapWidth;
						dy = (mDrawableRect.Height - mBitmapHeight * scale) * 0.5f;
					}

					mShaderMatrix.SetScale(scale, scale);
					mShaderMatrix.PostTranslate((int) (dx + 0.5f) + mDrawableRect.left, (int) (dy + 0.5f) + mDrawableRect.top);

					mBitmapShader.setLocalMatrix(mShaderMatrix);
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
				}*/

	}
	
}


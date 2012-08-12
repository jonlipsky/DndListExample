using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;

namespace DndListExample
{
	[Activity (Label = "DndListExample", MainLauncher = true)]
	public class Activity1 : Activity, View.IOnDragListener
	{
		private ArrayAdapter<string> adapter1;
		private ListView listView;
		private TextView textView;

		protected override void OnCreate (Bundle savedInstanceState)
		{
        	base.OnCreate(savedInstanceState);
        	SetContentView(Resource.Layout.Main);
        	Initialize();
    	}

		public bool OnDrag(View view, DragEvent e)
		{
			var action = e.Action;
			switch (action) 
			{
                case DragAction.Entered:
                    view.SetBackgroundColor(Color.DarkGray);
                    break;
                case DragAction.Exited:
                    view.SetBackgroundColor(Color.Transparent);
                    break;
                case DragAction.Started:
                    return HandleDragStart(e);
                case DragAction.Drop:
                    view.SetBackgroundColor(Color.Transparent);
                    return HandleDrop(e);
            }

            return false;
		}

	    private bool HandleDragStart(DragEvent evt) 
		{
			var clipDesc = evt.ClipDescription;
	        if (clipDesc != null) 
			{
	            return clipDesc.HasMimeType(ClipDescription.MimetypeTextPlain);
	        }
	        return false;
	    }

	    private bool HandleDrop(DragEvent evt) 
		{
			var data = evt.ClipData;;
	        if (data != null) 
			{
	            if (data.ItemCount > 0) 
				{
	                var item = data.GetItemAt(0);
					var textData = item.Text;
					string[] parts = textData.Split(new char[] {':'});
	                int index = int.Parse(parts[1]);
	                String listItem = parts[0];
	                HandleDropComplete(listItem, index);
	                return true;
	            }
	        }
	        return false;
	    }

	    private void HandleDropComplete(String listItem, int index) 
		{
	        adapter1.Remove(listItem);
	        adapter1.NotifyDataSetChanged();
	        textView.Text = listItem;
	    }

	    private void Initialize() 
		{
	        listView = (ListView) FindViewById(Resource.Id.listView);
	        textView = (TextView) FindViewById(Resource.Id.textView);

	        adapter1 = new ArrayAdapter<String>(this, global::Android.Resource.Layout.SimpleExpandableListItem1);
	        adapter1.Add("Item 1");
	        adapter1.Add("Item 2");
	        adapter1.Add("Item 3");
	        adapter1.Add("Item 4");
	        adapter1.Add("Item 5");
	        adapter1.Add("Item 6");
	        adapter1.Add("Item 7");
	        adapter1.Add("Item 8");
	        adapter1.Add("Item 9");
	        adapter1.Add("Item 10");

	        listView.SetAdapter(adapter1);

			listView.ItemLongClick += delegate(object sender, AdapterView.ItemLongClickEventArgs e) 
			{
				var view = (TextView)e.View;
				var title = view.Text;
				var textData = string.Format("{0}:{1}",title,e.Position);
                var data = ClipData.NewPlainText(title, textData);
                view.StartDrag(data, new SolidShadow(view, Color.Gray), null, 0);
			};

			textView.SetOnDragListener(this);
	    }
	}

	public class SolidShadow : View.DragShadowBuilder
	{
		private readonly float width;
		private readonly float height;
		private readonly Paint paint;

		public SolidShadow(View view, Color color) : base(view)
		{
			width = view.Width;
			height = view.Height;

		 	paint = new Paint();
			paint.Color = color;
			paint.SetStyle(Paint.Style.Fill);
		}

		public override void OnDrawShadow (Canvas canvas)
		{
			base.OnDrawShadow (canvas);

			canvas.DrawRect(0,0,width, height, paint);
        	View.Draw(canvas);
		}
	}
}



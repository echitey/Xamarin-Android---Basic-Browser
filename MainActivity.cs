using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;
using Android.Graphics;

namespace BasicBrowser
{
    [Activity(Label = "BasicBrowser", MainLauncher = true, Icon = "@drawable/icon", Theme ="@android:style/Theme.Black.NoTitleBar")]
    public class MainActivity : Activity
    {
        private EditText edtUrl;
        private WebView webView;
        private MyWebViewClient webViewClient;
        private ProgressBar progressBar;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            webViewClient = new MyWebViewClient();
            
            edtUrl = FindViewById<EditText>(Resource.Id.edtUrl);
            webView = FindViewById<WebView>(Resource.Id.browser);
            progressBar = FindViewById<ProgressBar>(Resource.Id.loading);

            webViewClient.showIt += (bool show) =>
            {
                if (show)
                {
                    progressBar.Visibility = ViewStates.Visible;
                }else
                {
                    progressBar.Visibility = ViewStates.Invisible;
                }

            };

            webView.Settings.JavaScriptEnabled = true;
            webView.LoadUrl("http://google.com");
            webView.SetWebViewClient(webViewClient);

            edtUrl.Click += (object sender, EventArgs e) =>
            {
                var url = edtUrl.Text;
                webViewClient.ShouldOverrideUrlLoading(webView, url);
            };
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            if(e.KeyCode == Keycode.Back)
            {
                webView.GoBack();
            }

            return true;
        }
    }

    public class MyWebViewClient : WebViewClient
    {
        public delegate void showProgressBar(bool show);
        public showProgressBar showIt;

        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            view.LoadUrl(url);
            return true;
        }

        public override void OnPageStarted(WebView view, string url, Bitmap favicon)
        {
            showIt.Invoke(true);
            base.OnPageStarted(view, url, favicon);
        }

        public override void OnPageFinished(WebView view, string url)
        {
            showIt.Invoke(false);
            base.OnPageFinished(view, url);
        }
    }
}


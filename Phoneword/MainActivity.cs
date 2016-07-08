using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Phoneword
{
    [Activity(Label = "Phone Word", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            var phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberTex);
            var translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var callButton = FindViewById<Button>(Resource.Id.CallButton);

            callButton.Enabled = false;
           
            var translatedNmber = string.Empty;
            translateButton.Click += (object sender, EventArgs e) =>
             {
                 translatedNmber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
                 if (string.IsNullOrWhiteSpace(translatedNmber))
                 {
                     callButton.Text = "Call";
                     callButton.Enabled = false;
                 }
                 else
                 {
                     callButton.Text = string.Format("Call {0}", translatedNmber);
                     callButton.Enabled = true;
                 }
             };

            callButton.Click += (object sender, EventArgs e) =>
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage(string.Format("Call {0}?", translatedNmber));
                callDialog.SetNeutralButton("Call", delegate
                 {
                     var callIntent = new Intent(Intent.ActionCall);
                     callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNmber));
                     StartActivity(callIntent);
                 });

                callDialog.SetNegativeButton("Cancel", delegate { });

                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            };
        }
        
    }
}


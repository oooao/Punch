using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using people_errandd.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using people_errandd.Models;
using Xamarin.Essentials;
using System.Text.RegularExpressions;
using Plugin.SharedTransitions;

namespace people_errandd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        readonly InformationViewModel informationViewModel = new InformationViewModel();
        bool allowTap = true;
        Regex regexemail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        
        Regex regexphone = new Regex( @"^09[0-9]{8}$");
        
        public AboutPage()
        {
            //this.BindingContext = new InformationViewModel();
            InitializeComponent();            
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = Color.FromHex("#EDEEEF");
            ((NavigationPage)Application.Current.MainPage).BarTextColor = Color.FromHex("#555555");           
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            name.Text = Preferences.Get("UserName", "");    
           // Console.WriteLine(Preferences.Get("HashAccount", ""));
            information inf = await informationViewModel.GetInformation(Preferences.Get("HashAccount", ""));
            //Console.WriteLine(" "+inf.name + inf.department + inf.email);
            try
            {
                jobTitle.Text = inf.jobtitle;
                department.Text = inf.department;
                phone.Text = inf.phone;
                email.Text = inf.email;
            }
            catch (Exception)
            {
            }
                                   
        }
        void RefButtonClicked(object sender, EventArgs e)
        {         
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    Navigation.PopModalAsync(true);
                }
            }
            finally
            {
                allowTap = true;
            }           
        }
        /*
        async void Image_clicked(System.Object sender,System.EventArgs e)
        {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions{
                Title = "選一張照片吧!"
            });

            var stream = await result.OpenReadAsync();

            resultImage.Source = ImageSource.FromStream(() => stream);
        }
        */
       /*
        async void Image_clicked(Object sender, System.EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        await DisplayAlert("無法使用", "現在無法使用", "OK");
                        return;
                    }

                    var mediaOptions = new PickMediaOptions()
                    {
                        PhotoSize = PhotoSize.Full
                    };
                    var selectedImageFile = await CrossMedia.Current.PickPhotoAsync(mediaOptions);

                    if (resultImage == null)
                    {
                        await DisplayAlert("無法使用", "現在無法使用", "OK");
                        return;
                    }
                    resultImage.Source = ImageSource.FromStream(() => selectedImageFile.GetStream());
                }
            }
            finally
            {
                allowTap = true;
            }         
        }
        */
        private async void ConfirmButton(object sender, EventArgs e)
        {
            
            try
            {
                if (allowTap)
                {
                    allowTap = false;
                    if (string.IsNullOrWhiteSpace(phone.Text))
                    {
                        phone.Text = "";
                    }
                            if (await informationViewModel.UpdateInformationRecord(name.Text, phone.Text, email.Text) && await informationViewModel.ConfirmEmail(email.Text))
                            {
                                Preferences.Set("phone", phone.Text);
                                Preferences.Set("email", email.Text);
                                await DisplayAlert("", "修改完成", "確認");                                
                            }
                            else
                            {
                                await DisplayAlert("錯誤", "信箱重複或是網路錯誤","確認");
                            }                                                                                 
                    await Navigation.PopAsync();
                }
            }
            finally
            {
                Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                {
                    allowTap = true;
                    return false;
                });
            }
        }

        private void Email_Unfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(email.Text))
            {
                Match matchemail = regexemail.Match(email.Text);
                if (!matchemail.Success)
                {
                    emailValidator.Text = "格式錯誤";
                    Confirm.IsEnabled = false;
                }
                else
                {
                    emailValidator.Text = "";
                    Confirm.IsEnabled = true;
                }               
            }
            else
            {
                emailValidator.Text = "信箱不可為空";
                Confirm.IsEnabled = false;

            }
        }

        private void Phone_Unfocused(object sender, FocusEventArgs e)
        {
            if (!string.IsNullOrEmpty(phone.Text))
            {
                Match matchphone = regexphone.Match(phone.Text);
                if (!matchphone.Success)
                {
                    phoneValidator.Text = "格式錯誤";
                    Confirm.IsEnabled = false;
                }
                else
                {
                    phoneValidator.Text = "";
                    Confirm.IsEnabled = true;
                }
            }
            else
            {
                phoneValidator.Text = "";
                Confirm.IsEnabled = true;
            }           
        }

        private async void LogOut(object sender, EventArgs e)
        {
            LogOutButton.BackgroundColor = Color.FromHex("#CA2D2D");
            try
            {
                if(allowTap)
                {
                   allowTap = false;
                    bool answer = await DisplayAlert("登出","確定要進行登出？","確認","取消");
                    if (answer)
                    {
                        App.Current.MainPage = new SharedTransitionNavigationPage(new LoginPage());
                        await Navigation.PopToRootAsync();
                        Preferences.Remove("HashAccount");                       
                    }
                    
                }
            }
            finally
            {
                allowTap = true;                
            }
        }
        void OnLogOutButtonPressed(object sender, EventArgs args)
        {
            LogOutButton.BackgroundColor = Color.FromHex("#972121");

        }

    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomerDatabaseAPI.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using RestSharp;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string URL = "http://localhost:53590/";
        private static RestClient client = new RestClient(URL);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SearchByAcctNumBtn_Click(object sender, RoutedEventArgs e)
        {
            errorMessages.Text = string.Empty;
            RestRequest request = new RestRequest("api/search/{id}");
            request.AddUrlSegment("id", SearchBox.Text);
            RestResponse response = client.Execute(request);

            Customer customer = JsonConvert.DeserializeObject<Customer>(response.Content);
            
            if (customer == null)
            {
                errorMessages.Text = "Customer not found!";
            }
            else
            {
                PopulateCustomerData(customer);
                errorMessages.Text = "Customer found!";
            }

        }

        private void GenerateDatabaseBtn_Click(object sender, RoutedEventArgs e)
        {
            RestRequest request = new RestRequest("api/data/generate");
            RestResponse response = client.Execute(request);
            MessageBox.Show("Data Successfully Generated");
        }

        private void PopulateCustomerData(Customer customer)
        {
            try
            {
                AcctNumBox.Text = customer.AccountNumber.ToString();
                FirstNameBox.Text = customer.FirstName;
                LastNameBox.Text = customer.LastName;
                BalanceBox.Text = customer.Balance.ToString();
                Pin_Number_Box.Text = customer.PinNumber;
                ProfilePicImage.Source = GetImageFromByteArray(customer.ProfilePicture);
            }
            catch (NullReferenceException)
            {
                errorMessages.Text = "Customer not found!";
            }
            
        }

        private BitmapImage GetImageFromByteArray(byte[] byteArray)
        {
            using (var ms = new System.IO.MemoryStream(byteArray))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();

                return image;
            }
        }

        private void AddCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            errorMessages.Text = string.Empty;
            RestRequest request = new RestRequest("api/search", Method.Post);
            Customer customer = CreateCustomerByInput();

            if (customer != null)
            {
                request.AddJsonBody(JsonConvert.SerializeObject(customer));
                RestResponse response = client.Execute(request);

                Customer returnCustomer = JsonConvert.DeserializeObject<Customer>(response.Content);

                if (returnCustomer == null)
                {
                    errorMessages.Text = "Existing customer has same Account Number! Customer not added.";
                }
                else
                {
                    errorMessages.Text = "Customer added!";
                }
            }
        }

        private void DeleteByAcctNumBtn_Click(object sender, RoutedEventArgs e)
        {
            errorMessages.Text = string.Empty;
            RestRequest request = new RestRequest("api/search/{id}", Method.Delete);
            request.AddUrlSegment("id", DeleteBox.Text);
            RestResponse response = client.Execute(request);

            Customer customer = JsonConvert.DeserializeObject<Customer>(response.Content);

            if (customer == null)
            {
                errorMessages.Text = "Customer not found! Delete failed.";
            }
            else
            {
                errorMessages.Text = "Customer deleted!";
            }
        }

        private void UpdateCustomerBtn_Click(object sender, RoutedEventArgs e)
        {
            errorMessages.Text = string.Empty;
            int id = Int32.Parse(AcctNumBox.Text);
            Customer customer = CreateCustomerByInput();

            if (customer != null)
            {
                RestRequest request = new RestRequest("api/search/{id}", Method.Put);
                request.AddUrlSegment("id", id);
                request.AddJsonBody(JsonConvert.SerializeObject(customer));
                RestResponse response = client.Execute(request);

                Customer returnCustomer = JsonConvert.DeserializeObject<Customer>(response.Content);

                if (returnCustomer == null)
                {
                    errorMessages.Text = "Customer not found! Update failed.";
                }
                else
                {
                    errorMessages.Text = "Customer data updated!";
                }
            }
            
        }
        private Customer CreateCustomerByInput()
        {
            try
            {
                if (ProfilePicImage.Source == null)
                {
                    errorMessages.Text = "Please upload a profile picture.";
                    return null;
                }
                else
                {
                    Customer customer = new Customer();
                    customer.AccountNumber = Int32.Parse(AcctNumBox.Text);
                    customer.FirstName = FirstNameBox.Text;
                    customer.LastName = LastNameBox.Text;
                    customer.PinNumber = Pin_Number_Box.Text;
                    customer.Balance = (decimal?)Double.Parse(BalanceBox.Text);
                    customer.ProfilePicture = Converter(ProfilePicImage.Source);
                    return customer;
                }  
            }
            catch (FormatException)
            {
                errorMessages.Text = "Please check format of entered fields.";
                return null;
            }
        }

        private void AddPictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ProfilePicImage.Source = new BitmapImage(new Uri(op.FileName));
            }
        }

        private byte[] Converter(ImageSource bitmap)
        {
            byte[] buffer = null;
            var bitmapSource = bitmap as BitmapSource;

            if (bitmap != null)
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    buffer = stream.ToArray();
                }
            }
            return buffer;
        }
    }
}

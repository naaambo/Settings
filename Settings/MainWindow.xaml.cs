using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
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
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Settings.Annotations;

namespace Settings
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // load the json configration file

            // iterate through the XML settings file and/or web.config checking if the node exists (has to match the json conf file)

            // render the data in the grid?table?treeview?

            // ex. checkbox for boolean


            var jsonData = @"{
                'appSettings': [
                    {
                        'key': 'aspnet: UseTaskFriendlySynchronizationContext',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'webpages: Version',
                        'type': 'int'
                    },
                    {
                        'key': 'webpages: Enabled',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'PreserveLoginUrl',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'ClientValidationEnabled',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'UnobtrusiveJavaScriptEnabled',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'RestServiceUrl',
                        'type': 'string'
                    },
                    {
                        'key': 'CoDataType',
                        'type': 'string'
                    },
                    {
                        'key': 'SettingsVirtualDirectory',
                        'type': 'string'
                    },
                    {
                        'key': 'SettingsFile',
                        'type': 'string'
                    },
                    {
                        'key': 'isCrossDomainEnabled',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'cacheTimeToLive',
                        'type': 'int'
                    },
                    {
                        'key': 'hierarchicalReadingRooms',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'renameReadingRooms',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'relatedNews',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'relatedActions',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.participant.form.details',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.signatory.form.details',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.readingroom.form.details',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.readingroom.form.viewers',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.meeting.form.details',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.resolution.form.details',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.agenda.form.details',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.document.form.details',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.news.form.details',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'accordion.action.form.details',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'agendaContributors',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'enable.minutes.phase2.features',
                        'type': 'Boolean'
                    },
                    {
                        'key': 'connect.version.build',
                        'type': 'string'
                    },
                    {
                        'key': 'json.serializer.log.level',
                        'type': 'string'
                    }
                ]
            }";

            WebConfig x = JsonConvert.DeserializeObject<WebConfig>(jsonData, new ConfigConverter());
            // load in memory the web.config as xml and read the values from the file using the keys defined in the webConfig class


            var settingsNodes = XDocument.Load(@"D:\Source\Connect\v1.9b\Task-Sam\OnBoardNextGeneration\OnBoard\Web.config").Element("configuration").Element("appSettings");

            var addNodes = settingsNodes.Elements("add");

            foreach (var appSetting in x.AppSettings)
            {
                var node = addNodes.Where(y => y.Attribute("key").Value == appSetting.Key).FirstOrDefault();

                if (node != null)
                {
                    appSetting.Value = node.Attribute("value").Value;
                }
            }

            Dispatcher.Invoke(() =>
            {
                MyDataGrid.ItemsSource = x.AppSettings;
            });

        }

        public class ConfigConverter : CustomCreationConverter<WebConfig>
        {

            public override WebConfig Create(Type objectType)
            {
                return new WebConfig();
            }

        }

        public class WebConfigData
        {

        }

        public class WebConfigAppSetting : INotifyPropertyChanged
        {
            public string KeyValue = String.Empty;
            public string TypeValue = String.Empty;
            public object ValueValue = null;
            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }

            public string Key
            {
                get { return this.KeyValue; }
                set
                {
                    if (value != this.KeyValue)
                    {
                        this.KeyValue = value;
                        OnPropertyChanged();
                    }
                }
            }
            public string Type
            {
                get { return this.TypeValue; }
                set
                {
                    if (value != this.TypeValue)
                    {
                        this.TypeValue = value;
                        OnPropertyChanged();
                    }
                }
            }

            public object Value
            {
                get {
                    return this.ValueValue;
                }
                set
                {
                    if (value == this.ValueValue) return;
                    if (this.TypeValue == "Boolean")
                    {
                        try
                        {
                            Convert.ToBoolean(value);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Please Enter a valid Boolean Value");
                            return;
                        }
                    } 
                    this.ValueValue = value;
                    OnPropertyChanged();
                }
            }

        }

        public class WebConfig
        {

            public WebConfigAppSetting this[string key]
            {
                get { return AppSettings.FirstOrDefault(x => x.Key == key); }
            }

            public WebConfigAppSetting Get(string key)
            {
                return AppSettings.FirstOrDefault(x => x.Key == key);
            }

            public List<WebConfigAppSetting> AppSettings { get; set; }
        }
        }    
}

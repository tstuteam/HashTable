using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GuiApp.MVVM.Model;

namespace GuiApp.MVVM.ViewModel;

class MainViewModel
{
    public ObservableCollection<ContactModel> Contacts { get; set; } = new();

    public MainViewModel()
    {
        Contacts.Add(new()
        {
            Name = "Contact1",
            PhoneNumber = "+7-800-555-35-35",
            Picture = "https://i.imgur.com/IXXXL0L.png"
        });

        Contacts.Add(new()
        {
            Name = "Contact2",
            PhoneNumber = "+7-910-412-21-99",
            Picture = "https://i.imgur.com/eG4FcOp.png"
        });

        Contacts.Add(new()
        {
            Name = "Contact3",
            PhoneNumber = "+7-777-777-77-77",
            Picture = "https://i.imgur.com/v4ob8jg.png"
        });

        Contacts.Add(new()
        {
            Name = "Contact4",
            PhoneNumber = "+7-481-491-31-12",
            Picture = "https://i.imgur.com/6zWmHUA.png"
        });

        Contacts.Add(new()
        {
            Name = "Contact5",
            PhoneNumber = "+7-823-013-41-91",
            Picture = "https://i.imgur.com/LuVViQy.png"
        });
    }
}

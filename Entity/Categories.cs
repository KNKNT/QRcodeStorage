using System.Windows.Controls;

namespace QRcodeStorage.Models
{
    internal class Categories
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public void LoadComboBoxes(ComboBox comboBox, List<(int Id, string Text)> data, string defaultText = "--Не выбрано--")
        {
            var items = new List<object>();
            items.Add(new { Id = 0, Text = defaultText });

            foreach (var item in data)
            {
                items.Add(new { item.Id, item.Text });
            }

            comboBox.ItemsSource = items;
            comboBox.DisplayMemberPath = "Text";
            comboBox.SelectedValuePath = "Id";
            comboBox.SelectedIndex = 0;
        }
    }
}

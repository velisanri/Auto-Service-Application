using PersonelService.Entities;

namespace PersonelService.WebUI.Models
{
    public class HomePageViewModel
    {
        public List<Slider>? Sliders { get; set; }
        public List<Arac>? Araclar { get; set; }

        public Arac? Arac { get; set; }
        public Musteri? Musteri { get; set; }

    }
}

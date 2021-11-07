namespace checkpoint2 {
    class Product {
        public string category { get; set; }
        public string name { get; set; }
        public double price { get; set; }

        public Product (string category, string name, double price) {
            this.category = category;
            this.name = name;
            this.price = price;
        }
    }
}

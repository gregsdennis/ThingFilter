Think of it as Google for your objects.

[![littlecrabsolutions MyGet Build Status](https://www.myget.org/BuildSource/Badge/littlecrabsolutions?identifier=526ca85a-64e9-4255-9774-61bb974a3487)](https://www.myget.org/)

## An Example

Suppose we have a product list that we need to filter based on input into a search field.

Here's the product:

	class Product
	{
		public string Sku { get; set; }
		public string Title { get; set; }
		public decimal UnitPrice { get; set; }
	}

Simplistically, we could perform a `Contains()` on the individual properties:

	public IEnumerable<Product> Filter(this IEnumerable<Product> products,
									   string filter)
	{
		return products.Where(p => p.Sku.Contains(filter) ||
                                   p.Title.Contains(filter));
	}

To get a little more sophisticates, we could break the filter into individual words and perform the above search on each one.

But this approach has some drawbacks:

1. It requires a lot of coding effort, especially when your model may change or you need to filter several lists, each with their own kind of object.
2. In the example above, the user cannot filter on `UnitPrice` unless we add `p.UnitPrice.ToString().Contains(filter)` which feels clunky.

That's where ThingFilter comes in.

## Usage

First, we need to create an instance of ThingFilter based on the type of object we want to filter.  Then we configure it.

### Choose your filter criteria

In order for the filter to work, we need to tell it the data to analyze via the `MatchOn()` method.  The sole parameter for this method is a function that takes the object and returns a value.  This value will be converted to a string and compared to a word in the query.

	var filter = new ThingFilter<Product>()
                        .MatchOn(p => p.Sku)
                        .MatchOn(p => p.Title)
                        .MatchOn(p => p.Price);

### Optionally select a case-sensitive filter

The default behavior of the ThingFilter is a case-insensitive comparison.  If you prefer a case-sensitive comparison, you can use the `CaseSensitive()` method.

	filter.CaseSensitive();

### Sort by relevance

By default, the filter will return matches in the same order as the collection provided to it.  Optionally, you can sort by relevance by using the `SortByRelevance()` method.  This will cause the filter to rank each match and sort the results in a descending order, eliminating items which have no matches.

### Getting results

Once configured, you'll want results.  To get your results, pass your collection into the `Apply()` method.  This will yield an `IEnumerable<T>` with only the items which match your filter.

	var results = filter.Apply(allProducts, "toy rubik");

Additionally, just like the Linq extension methods, ThingFilter uses deferred execution, so once applied the results will update as the collection updates.
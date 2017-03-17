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

## Query Options

	simple "quoted substring" tagged:6

The above query string shows the available syntaxes supported by ThingFilter.  Let's break it down to see how each token is applied.

The first two are pretty easy.

- The `simple` token is a single word that matches on a "contains" basis.
- The `quoted substring` is multiple words that matches on a "contains" basis.

Both of these work on any of the configured values of an item.

The `tagged:6` on is a bit more complex.  In configuring the filter, the tag `tagged` has been set to search on a particular value of an item in the collection.  This means that even if another configured value produces a match, the match won't be registered.

Furthermore, a tagged value may be configured to match only if the tag is present.  If this were the case for `tagged:6`, then a mere `6` will not match on the value.

## Configuration

Configuring the ThingFilter is performed through the `MatchOn()` method.  This method requires a function to return the value on which to filter.  Once obtained, the value will be converted to a string (via `ToString()`) so that the comparison to the query token can occur.

In it's simplest form, it will be matched against all untagged query tokens:

	var filter = new ThingFilter<Product>().MatchOn(p => p.Title);

The function you use doesn't have to be a property; it could be a field, or even a method that returns the desired value.  The following are also valid:

	filter.MatchOn(p => p.GetTitle());
	filter.MatchOn(p => p.Title.Length);

To allow the user to specify a tag, you can provide it as a second parameter:

	var filter = new ThingFilter<Product>().MatchOn(p => p.Title, "title");

The tag doesn't have to match the name of the value, although it's sometimes helpful.  It's important to remember, though, that tags in the query token must be exactly equal to the value specified here.

Finally, if you'd like the tag to be required, pass a `true` as the third parameter:

	var filter = new ThingFilter<Product>().MatchOn(p => p.Title, "title", true);

><small>**NOTE** The `MatchOn()` method will throw an exception if you specify that the tag is required without specifying a non-empty, non-whitespace tag.</small>

## Getting results

Once configured, you'll want results.  To get your results, pass your collection into the `Apply()` method.  This will yield an `IEnumerable<T>` with only the items which match your filter.

	var results = filter.Apply(allProducts, "toy rubik");

Additionally, just like the Linq extension methods, ThingFilter uses deferred execution, so once applied the results will update as the collection updates.
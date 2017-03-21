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

To get a little more sophisticated, we could break the filter into individual words and perform the above search on each one.

But this approach has some drawbacks:

1. It requires a lot of coding effort, especially when your model may change or you need to filter several lists, each with their own kind of object.
2. In the example above, the user cannot filter on `UnitPrice` unless we add `p.UnitPrice.ToString().Contains(filter)` which feels clunky.

That's where ThingFilter comes in.

## Query Options

	simple "quoted substring" tagged:value 42 true

The above query string shows the available syntaxes supported by ThingFilter.  Let's break it down to see how each token is applied.

### Simple queries

The first two are pretty easy.

- The `simple` token is a single word that matches on a "contains" basis.
- The `quoted substring` is multiple words that matches on a "contains" basis.

Both of these work on any of the configured values of an item.

### Tagged queries

The `tagged:value` on is a bit more complex.  In configuring the filter, the tag `tagged` has been set to search on a particular value of an item in the collection.  This means that even if another configured value produces a match, the match won't be registered.

Furthermore, a tagged value may be configured to match only if the tag is present.  If this were the case for `tagged:value`, then a mere `value` will not match on the value.

### Interpreted data types

For the `42` and `true` tokens, if the configured value is of a numeric or boolean type, ThingFilter will attempt to parse the token into the pertinent type.  If the parse succeeds, then it will perform the comparison in that type.  Otherwise, the value is converted to a string via `ToString()` and the comparison proceeds as above.

><small>**NOTE** You may experience some unexpected behavior if your objects don't override `ToString()`.  The default implementation (from `object`) simply returns the type name.  This would cause matching on that value rather than what you intent. To remedy this, be sure you either override `ToString()` to a queryable value or configure your match to use some other value.</small>

### Out-of-the-box operations

ThingFilter also supports the following operators:

- `:` Contains
- `=` Equal To
- `<>` Not Equal To
- `<` Less Than
- `<=` Less Than Or Equal To
- `>` Greater Than
- `>=` Greater Than Or Equal To

Please note that the *Contains* operator is only meaningful for string values, and the inequality operators (*Less Than*, etc.) are not meaningful for boolean values.  When these operators are used on meaningless values (e.g. `<=true` for a boolean value), they will never be matched.

><small>**NOTE** The query token `<=true` is perfectly acceptable as a string comparison.</small>

### Custom operations

The operations recognized by ThingFilter can be customized through the use of the `AddEvaluator()` and `RemoveEvaluator()` methods.

`AddEvaluator()` takes an implementation of `IMatchEvaluator`.  This interface defines the matching algorithm for strings, numbers (represented by `double`), and boolean values as well as the operator that identifies it.  At a minimum, the string matching should be implemented.  If the other data types are not valid for your evaluator, they should simply return `false`.

`RemoveEvaluator()` takes a string representing the operator to be removed.  For example, if you want your filter to not support the *Not Equal To* operator, you would pass `"<>"`.

## Configuration

Configuring the ThingFilter is performed primarily through the `MatchOn()` method.  This method requires a function to return the value on which to filter.  Once obtained, the value will be checked for boolean and numeric types.  If the value not one of these types, it is converted to a string (via `ToString()`) and matching continues.

In it's simplest form, a value will be matched against all untagged query tokens:

	var filter = new ThingFilter<Product>().MatchOn(p => p.Title);

The function you use doesn't have to return a property value; it could return a field, or even call a method that returns the desired value.  The following are also valid:

	filter.MatchOn(p => p.GetTitle());
	filter.MatchOn(p => p.Title.Length);

To allow the user to specify a tag, you can provide it as a second parameter:

	var filter = new ThingFilter<Product>().MatchOn(p => p.Title, "title");

The tag doesn't have to match the name of the value, although it's sometimes helpful.  It's important to remember, though, that tags in the query token must be exactly equal to the value specified here.  Also, these tags will be entered by your user, so take care to use something they will easily remember.  In general, short tags are better.

If you'd like the tag to be required, pass a `true` as the third parameter:

	var filter = new ThingFilter<Product>().MatchOn(p => p.Title, "title", true);

><small>**NOTE** The `MatchOn()` method will throw an exception if you specify that the tag is required without specifying a non-empty, non-whitespace tag.</small>

Finally, if you would like to add a particular weighting to matching a particular value, you can supply the weight as the final argument.  The default weight is 1.

	// creates matching on the Title property, requires the tag "title", and adds a weighting of 2.
	var filter = new ThingFilter<Product>().MatchOn(p => p.Title, "title", true, 2);
	// creates matching on the Title property and adds a weighting of 2, no tag specified.
	var filter = new ThingFilter<Product>().MatchOn(p => p.Title, weight: 2);

### Case sensitivity

By default, all string comparisons are case-insensitive.  To specify a case-sensitive comparison, you can use the `CaseSensitive()` method.

## Getting results

Once configured, you'll probably want results.  To get them, pass your collection into the `Apply()` method.  This will yield an `IEnumerable<IFilterResult<T>>` with only the items which match your filter along with a few extra data points.

	var results = filter.Apply(allProducts, "toy rubik");

The `IFilterResult<T>` object will contain the item that was matched as well as the item's score.  (Messages regarding the match is a pending feature.)  The score is determined by how many tokens were matched for that item.  This is useful for when you want to get a ranked list.

Additionally, the return value of ThingFilter is a Linq query at its core, so it uses deferred execution: once applied the results will update as the collection updates.  If you don't want to enumerate the query multiple times, remember to call `ToList()` on the results.
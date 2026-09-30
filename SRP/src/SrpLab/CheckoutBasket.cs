namespace SrpLab;

public sealed class CheckoutBasket
{
    private readonly List<(string Sku, decimal Price, int Qty)> _lines = new();
    private string? _couponRaw;
    private bool _giftWrap;

    public void AddLine(string sku, decimal price, int qty)
    {
        if (qty <= 0)
            throw new ArgumentOutOfRangeException(nameof(qty));

        _lines.Add((sku, price, qty));
    }

    public void ApplyCouponText(string? couponText)
    {
        _couponRaw = couponText;
    }

    public void EnableGiftWrap()
    {
        _giftWrap = true;
    }

    public decimal SubTotal()
    {
        return _lines.Sum(l => l.Price * l.Qty);
    }

    public decimal GrandTotal()
    {
        var discount = new CouponDiscount().Calculate(_couponRaw, SubTotal());

        var total = SubTotal() - discount;

        if (_giftWrap)
            total += GiftWrap.Price;

        return Math.Max(0m, total);
    }

    public string GiftMessageCard(string fromName)
    {
        return new GiftMessageGenerator().Generate(
            fromName,
            _lines.Select(l => l.Sku),
            GrandTotal());
    }

    public string AuthorizePaymentStub(string cardLast4)
    {
        return new PaymentAuthorizer().Authorize(
            GrandTotal(),
            cardLast4,
            _lines.Count);
    }
}

public sealed class CouponDiscount
{
    public decimal Calculate(string? couponText, decimal subtotal)
    {
        if (string.IsNullOrWhiteSpace(couponText))
            return 0m;

        var t = couponText.Trim().ToUpperInvariant();

        if (t.StartsWith("SAVE") &&
            int.TryParse(t[4..], out var pct) &&
            pct is > 0 and <= 50)
        {
            return Math.Round(subtotal * pct / 100m, 2);
        }

        if (t.Contains("FREESHIP"))
            return 0m;

        if (t == "WELCOME10")
            return Math.Min(10m, subtotal);

        return 0m;
    }
}

public static class GiftWrap
{
    public const decimal Price = 4.99m;
}

public sealed class GiftMessageGenerator
{
    public string Generate(
        string fromName,
        IEnumerable<string> items,
        decimal total)
    {
        var itemList = string.Join(", ", items);

        return $"Dear friend,\n" +
               $"A gift from {fromName} awaits ({itemList}).\n" +
               $"Total surprise value: {total:C}\n";
    }
}

public sealed class PaymentAuthorizer
{
    public string Authorize(
        decimal total,
        string cardLast4,
        int itemCount)
    {
        var payload = $"{total:0.00}|{cardLast4}|{itemCount}";

        var hash = payload.GetHashCode();

        return $"AUTH-{Math.Abs(hash):X8}";
    }
}
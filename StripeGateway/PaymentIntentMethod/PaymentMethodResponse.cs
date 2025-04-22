using Newtonsoft.Json;
using Stripe;

namespace StripeGateway
{
    public class PaymentMethodResponse
    {
        public string Id { get; set; }

        //
        // Summary:
        //     This field indicates whether this payment method can be shown again to its customer
        //     in a checkout flow. Stripe products such as Checkout and Elements use this field
        //     to determine whether a payment method can be shown as a saved payment method
        //     in a checkout flow. The field defaults to “unspecified”. One of: always, limited,
        //     or unspecified.
        [JsonProperty("allow_redisplay")]
        public string AllowRedisplay { get; set; }


        [JsonProperty("billing_details")]
        public PaymentMethodBillingDetailsResponse BillingDetails { get; set; }


        [JsonProperty("card")]
        public PaymentMethodCardResponse Card { get; set; }

        //[JsonProperty("card_present")]
        //public PaymentMethodCardPresent CardPresent { get; set; }

        //[JsonProperty("cashapp")]
        //public PaymentMethodCashapp Cashapp { get; set; }

        //
        // Summary:
        //     Time at which the object was created. Measured in seconds since the Unix epoch.
        [JsonProperty("created")]
        public DateTime Created { get; set; }


        //
        // Summary:
        //     (ID of the Customer) The ID of the Customer to which this PaymentMethod is saved.
        //     This will not be set when the PaymentMethod has not been saved to a Customer.
        [JsonIgnore]
        public string CustomerId { get; set; }


        //[JsonProperty("customer_balance")]
        //public PaymentMethodCustomerBalance CustomerBalance { get; set; }



        //[JsonProperty("link")]
        //public PaymentMethodLink Link { get; set; }

        //
        // Summary:
        //     Has the value true if the object exists in live mode or the value false if the
        //     object exists in test mode.
        [JsonProperty("livemode")]
        public bool Livemode { get; set; }


        //
        // Summary:
        //     The type of the PaymentMethod. An additional hash is included on the PaymentMethod
        //     with a name matching this value. It contains additional information specific
        //     to the PaymentMethod type. One of: acss_debit, affirm, afterpay_clearpay, alipay,
        //     alma, amazon_pay, au_becs_debit, bacs_debit, bancontact, blik, boleto, card,
        //     card_present, cashapp, customer_balance, eps, fpx, giropay, gopay, grabpay, id_bank_transfer,
        //     ideal, interac_present, kakao_pay, klarna, konbini, kr_card, link, mb_way, mobilepay,
        //     multibanco, naver_pay, oxxo, p24, payco, paynow, paypal, payto, pix, promptpay,
        //     qris, rechnung, revolut_pay, samsung_pay, sepa_debit, shopeepay, sofort, swish,
        //     twint, us_bank_account, wechat_pay, or zip.
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("us_bank_account")]
        public PaymentMethodUsBankAccount UsBankAccount { get; set; }

        //[JsonProperty("wechat_pay")]
        //public PaymentMethodWechatPay WechatPay { get; set; }

        //[JsonProperty("zip")]
        //public PaymentMethodZip Zip { get; set; }
    }
}

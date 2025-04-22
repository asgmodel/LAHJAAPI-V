using LAHJAAPI.Dto;
using Dto.Stripe.Invoice;
using Newtonsoft.Json;

namespace StripeGateway
{
    public class InvoiceResponse
    {
        public string Id { get; set; }

        public string Object { get; set; }

        /// <summary>
        /// The country of the business associated with this invoice, most often the business
        /// creating the invoice.
        /// </summary>
        public string AccountCountry { get; set; }

        /// <summary>
        /// The public name of the business associated with this invoice, most often the business
        /// creating the invoice.
        /// </summary>
        public string AccountName { get; set; }

        [JsonProperty("amount_due")]
        public long AmountDue { get; set; }

        /// <summary>
        /// Amount that was overpaid on the invoice. Overpayments are debited to the customer's
        /// credit balance.
        /// </summary>
        [JsonProperty("amount_overpaid")]
        public long AmountOverpaid { get; set; }

        /// <summary>
        /// The amount, in cents (or local equivalent), that was paid.
        /// </summary>
        [JsonProperty("amount_paid")]
        public long AmountPaid { get; set; }

        /// <summary>
        /// The difference between amount_due and amount_paid, in cents (or local equivalent).
        /// </summary>
        [JsonProperty("amount_remaining")]
        public long AmountRemaining { get; set; }

        /// <summary>
        /// This is the sum of all the shipping amounts.
        /// </summary>
        [JsonProperty("amount_shipping")]
        public long AmountShipping { get; set; }

        /// <summary>
        /// Indicates the reason why the invoice was created.
        ///
        /// * <c>manual</c>: Unrelated to a subscription, for example, created via the invoice
        /// editor. * <c>subscription</c>: No longer in use. Applies to subscriptions from before
        /// May 2018 where no distinction was made between updates, cycles, and thresholds. *
        /// <c>subscription_create</c>: A new subscription was created. * <c>subscription_cycle</c>:
        /// A subscription advanced into a new period. * <c>subscription_threshold</c>: A
        /// subscription reached a billing threshold. * <c>subscription_update</c>: A subscription
        /// was updated. * <c>upcoming</c>: Reserved for simulated invoices, per the upcoming
        /// invoice endpoint.
        /// One of: <c>automatic_pending_invoice_item_invoice</c>, <c>manual</c>,
        /// <c>quote_accept</c>, <c>subscription</c>, <c>subscription_create</c>,
        /// <c>subscription_cycle</c>, <c>subscription_threshold</c>, <c>subscription_update</c>, or
        /// <c>upcoming</c>.
        /// </summary>
        [JsonProperty("billing_reason")]
        public string BillingReason { get; set; }



        /// <summary>
        /// The URL for the hosted invoice page, which allows customers to view and pay an invoice.
        /// If the invoice has not been finalized yet, this will be null.
        /// </summary>
        [JsonProperty("hosted_invoice_url")]
        public string HostedInvoiceUrl { get; set; }

        /// <summary>
        /// The link to download the PDF for the invoice. If the invoice has not been finalized yet,
        /// this will be null.
        /// </summary>
        [JsonProperty("invoice_pdf")]
        public string InvoicePdf { get; set; }

        /// <summary>
        /// The status of the invoice, one of <c>draft</c>, <c>open</c>, <c>paid</c>,
        /// <c>uncollectible</c>, or <c>void</c>. <a
        /// href="https://stripe.com/docs/billing/invoices/workflow#workflow-overview">Learn
        /// more</a>.
        /// One of: <c>draft</c>, <c>open</c>, <c>paid</c>, <c>uncollectible</c>, or <c>void</c>.
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }


        /// <summary>
        /// The individual line items that make up the invoice. <c>lines</c> is sorted as follows:
        /// (1) pending invoice items (including prorations) in reverse chronological order, (2)
        /// subscription items in reverse chronological order, and (3) invoice items added after
        /// invoice creation in chronological order.
        /// </summary>
        [JsonProperty("lines")]
        public CustomStripeList<InvoiceLineItem> Lines { get; set; }
    }
}

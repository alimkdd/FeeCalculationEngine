 Smart Fee Calculation Engine

The Smart Fee Calculation Engine is a flexible, extensible, and highly testable backend system designed to calculate transaction fees based on complex business rules and transaction context.
It supports multiple transaction types, layered fee modifiers, edge-case handling, and full auditability, making it suitable for financial and enterprise-grade systems.

The solution emphasizes clean architecture, SOLID principles, and maintainability, while addressing real-world analytical challenges such as rule conflicts, performance at scale, and date-dependent logic.

✨ Key Features

Multi-transaction fee calculation (Domestic, International, Withdrawal, Deposit)

Strategy Pattern for transaction-specific fee logic

Chain of Responsibility for fee modifiers

Configuration-driven rules (no hardcoded values)

Full audit logging for compliance

Preview and final calculation endpoints

Designed for scalability and testability

📌 Business Rules
Base Fee Rules

Domestic Transfers

≤ $100 → $1.50 flat fee

$100.01 – $1,000 → 1.5%

$1,000 → $15 + 0.5% above $1,000

International Transfers

$5 flat + 3%

+$2 SWIFT fee if amount > $5,000

Withdrawals

First 3 per month → Free

4th+ → $2.50

ATM → +$1.50

Deposits

Bank → Free

Card → 2.5% (min $0.50, max $25)

Modifier Rules (Applied After Base Fee)

Premium users → –25%

High-volume users (>50/month) → –10%

Promotional codes → Variable discount %

Weekend/Holiday → +$1 processing fee

First transaction ever → All fees waived

⚠️ Edge Case Handling

Fee capped at 10% of transaction amount

Minimum paid fee: $0.25

Discounts stack but never exceed 100%

Promo codes respect expiration dates

Monthly withdrawal count resets on the 1st

Business calendar used for weekends/holidays

Fees never go negative

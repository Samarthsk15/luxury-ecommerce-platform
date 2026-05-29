import sqlite3
import os

db_path = r"c:\Users\Darshan Kulkarni\.gemini\antigravity\scratch\luxury-ecommerce-platform\backend\OrderProductApi\shop.db"

conn = sqlite3.connect(db_path)
conn.row_factory = sqlite3.Row
cursor = conn.cursor()

# Get schema for Orders table
print("📋 Orders Table Schema:")
cursor.execute("PRAGMA table_info(Orders)")
columns = cursor.fetchall()
for col in columns:
    print(f"  - {col['name']} ({col['type']})")
print()

# Get schema for OrderItems table
print("📋 OrderItems Table Schema:")
cursor.execute("PRAGMA table_info(OrderItems)")
columns = cursor.fetchall()
for col in columns:
    print(f"  - {col['name']} ({col['type']})")
print()

# Get all users
print("👥 ALL USERS:")
print("=" * 80)
cursor.execute("SELECT Id, Email, Name, Role FROM Users")
users = cursor.fetchall()
if users:
    print(f"Total Users: {len(users)}")
    for user in users:
        print(f"  ✓ ID: {user['Id']} | {user['Name']} ({user['Email']}) | Role: {user['Role']}")
else:
    print("  No users found")
print()

# Get all orders
print("📦 ALL ORDERS & TRACKING:")
print("=" * 80)
cursor.execute("""
    SELECT o.Id, o.UserId, u.Email, u.Name, o.TotalAmount, o.Status
    FROM Orders o
    LEFT JOIN Users u ON o.UserId = u.Id
""")
orders = cursor.fetchall()
print(f"Total Orders: {len(orders)}")
if orders:
    print()
    for order in orders:
        print(f"Order #{order['Id']}:")
        print(f"  User: {order['Name']} ({order['Email']})")
        print(f"  Amount: ₹{order['TotalAmount']}")
        print(f"  Status: {order['Status']}")
        
        # Get order items
        cursor.execute("""
            SELECT oi.Id, oi.Quantity, oi.UnitPrice, p.Name, p.Category
            FROM OrderItems oi
            LEFT JOIN Products p ON oi.ProductId = p.Id
            WHERE oi.OrderId = ?
        """, (order['Id'],))
        items = cursor.fetchall()
        if items:
            print(f"  Items Ordered:")
            for item in items:
                total = item['Quantity'] * item['UnitPrice']
                print(f"    • {item['Name']} (Category: {item['Category']})")
                print(f"      Qty: {item['Quantity']} x ₹{item['UnitPrice']} = ₹{total}")
        print()
else:
    print("  No orders found")
print()

# Summary
print("📊 SUMMARY:")
print("=" * 80)
cursor.execute("SELECT COUNT(*) as count FROM Users WHERE Role != 'Admin'")
regular_users = cursor.fetchone()['count']
print(f"  Total Regular Users: {regular_users}")
print(f"  Total Admin Users: {len(users) - regular_users}")
print(f"  Total Orders: {len(orders)}")
cursor.execute("SELECT SUM(TotalAmount) as total FROM Orders")
total_revenue = cursor.fetchone()['total']
print(f"  Total Revenue: ₹{total_revenue if total_revenue else 0}")

conn.close()

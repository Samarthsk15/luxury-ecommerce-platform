import requests
import json

# Admin credentials
admin_email = "admin@shop.local"
admin_password = "Admin@123"
api_base = "http://localhost:5000"

print("🔐 Logging in as admin...")
login_resp = requests.post(
    f"{api_base}/api/auth/login",
    json={"email": admin_email, "password": admin_password}
)

if not login_resp.ok:
    print(f"❌ Login failed: {login_resp.text}")
    exit(1)

login_data = login_resp.json()
token = login_data.get("token")
print(f"✓ Admin logged in successfully")
print(f"  User: {login_data.get('name')} ({login_data.get('email')})")
print(f"  Role: {login_data.get('role')}")
print()

# Fetch all orders
print("📊 Fetching all orders and user tracking data...")
headers = {
    "Authorization": f"Bearer {token}",
    "Content-Type": "application/json"
}

orders_resp = requests.get(f"{api_base}/api/orders", headers=headers)

if not orders_resp.ok:
    print(f"❌ Failed to fetch orders: {orders_resp.text}")
    exit(1)

orders_data = orders_resp.json()

if isinstance(orders_data, list):
    total_orders = len(orders_data)
    print(f"✓ Total Orders Found: {total_orders}")
    print("=" * 80)
    print()
    
    if total_orders == 0:
        print("ℹ️  No orders found in the system yet.")
    else:
        print(json.dumps(orders_data, indent=2))
else:
    print(f"Response: {json.dumps(orders_data, indent=2)}")

print()
print("=" * 80)
print("Summary:")
print(f"  - Total Users with Orders: {len(set(o.get('userId') for o in orders_data if isinstance(orders_data, list)))}")
print(f"  - Total Orders: {len(orders_data) if isinstance(orders_data, list) else 0}")

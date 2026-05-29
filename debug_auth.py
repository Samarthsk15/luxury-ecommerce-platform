import requests
import json

admin_email = "admin@shop.local"
admin_password = "Admin@123"
api_base = "http://localhost:5000"

print("🔐 Step 1: Login as admin...")
login_resp = requests.post(
    f"{api_base}/api/auth/login",
    json={"email": admin_email, "password": admin_password}
)

print(f"Login status: {login_resp.status_code}")
login_data = login_resp.json()
print(f"Login response: {json.dumps(login_data, indent=2)}")
print()

if not login_resp.ok:
    print("❌ Login failed")
    exit(1)

token = login_data.get("token")
print(f"✓ Token received: {token[:50]}...")
print()

# Test direct headers
print("🔐 Step 2: Test with Bearer token...")
headers = {
    "Authorization": f"Bearer {token}",
    "Content-Type": "application/json"
}

print(f"Headers: {headers}")
print()

orders_resp = requests.get(f"{api_base}/api/orders", headers=headers)
print(f"Orders response status: {orders_resp.status_code}")
print(f"Orders response: {orders_resp.text[:500]}")

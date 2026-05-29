from pathlib import Path

path = Path(r'c:\Users\Darshan Kulkarni\\.gemini\\antigravity\\scratch\\luxury-ecommerce-platform\\frontend\\src\\pages\\CheckoutPage.vue')
text = path.read_text(encoding='utf-8')
old = '''const decodeEmailName = (email) => {
  if (!email || !email.includes('@')) return '';
  const localPart = email.split('@')[0].split('+')[0];
  let namePart = localPart.replace(/[\x00-\x1f\x7f]+/g, '');
  namePart = namePart.replace(/[\x00-\x1f\x7f]/g, '');
  namePart = namePart
    .replace(/[\x00-\x1f]/g, '')
    .replace(/[\n\t]/g, '')
    .replace(/[_\x00-\x1f]/g, ' ')
    .replace(/[\x00-\x1f-]+/g, ' ')
    .replace(/[._-]+/g, ' ')
    .replace(/\x08([a-z])/g, (match) => match.toUpperCase())
    .replace(/\x08([0-9]+)\x08/g, '')
    .trim();
  return namePart.replace(/\x08\x08er\x08/g, '').replace(/\x08Email\x08/g, '').trim() || '';
};
'''
new = '''const decodeEmailName = (email) => {
  if (!email || !email.includes('@')) return '';
  const localPart = email.split('@')[0].split('+')[0];
  const normalized = localPart
    .replace(/[_.-]+/g, ' ')
    .replace(/([a-z])([A-Z])/g, '$1 $2')
    .replace(/[0-9]+/g, ' ')
    .trim();
  const words = normalized
    .split(/\s+/)
    .filter(Boolean)
    .map((word) => word.charAt(0).toUpperCase() + word.slice(1).toLowerCase());
  return words.join(' ');
};
'''
if old not in text:
    raise RuntimeError('Old function block not found')
path.write_text(text.replace(old, new), encoding='utf-8')
print('patched')

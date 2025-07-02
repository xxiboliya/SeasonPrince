from PIL import Image
import os

# 需要切片的动作（不区分大小写）
actions = ['walk', 'idle', 'jump', 'hurt', 'spellcast']
size = 64
base = os.path.join('Assets', 'Sprites', 'lpc_teen_animations_2025-06-29T12-14-10', 'standard')
out = os.path.join('Assets', 'Sprites', 'lpc_teen_animations_2025-06-29T12-14-10', 'sliced')
os.makedirs(out, exist_ok=True)

# 自动检测所有PNG文件
all_files = os.listdir(base)
file_map = {f.lower(): f for f in all_files if f.lower().endswith('.png')}

for act in actions:
    fname = f'{act}.png'
    key = fname.lower()
    if key not in file_map:
        print(f'Not found: {fname}')
        continue
    img_path = os.path.join(base, file_map[key])
    img = Image.open(img_path)
    w, h = img.size
    frames = w // size
    for i in range(frames):
        crop = img.crop((i*size, 0, (i+1)*size, size))
        crop.save(os.path.join(out, f'{act}_{i}.png'))
    print(f'{act}: {frames} frames sliced.') 
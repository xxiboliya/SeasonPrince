import os
from PIL import Image

# 只处理以下动作组
actions = ['idle', 'walk', 'run', 'jump', 'spellcast', 'hurt']

# 输入目录
input_dir = os.path.join(os.path.dirname(__file__), '../Sprites/lpc_teen_animations_2025-06-29T12-14-10/standard')
# 输出目录
output_dir = os.path.join(os.path.dirname(__file__), '../Sprites/lpc_teen_animations_2025-06-29T12-14-10/sliced_group4')
os.makedirs(output_dir, exist_ok=True)

for action in actions:
    filename = f'{action}.png'
    img_path = os.path.join(input_dir, filename)
    if not os.path.exists(img_path):
        print(f'Skip: {filename} not found')
        continue
    img = Image.open(img_path)
    width, height = img.size
    group_height = height // 4
    # 提取最后一行（第四组）
    box = (0, group_height * 3, width, height)
    group4 = img.crop(box)
    # 保存到输出目录
    out_path = os.path.join(output_dir, f'{action}_group4.png')
    group4.save(out_path)
    print(f'Saved: {out_path}')

print('All done!') 
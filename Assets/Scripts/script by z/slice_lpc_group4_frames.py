import os
from PIL import Image

# 输入目录
input_dir = os.path.join(os.path.dirname(__file__), '../Sprites/lpc_teen_animations_2025-06-29T12-14-10/sliced_group4')

for filename in os.listdir(input_dir):
    if filename.lower().endswith('.png'):
        img_path = os.path.join(input_dir, filename)
        img = Image.open(img_path)
        width, height = img.size
        # 假设每帧宽度相等，帧数 = 宽度 // 高度
        frame_count = width // height
        out_dir = os.path.join(input_dir, os.path.splitext(filename)[0])
        os.makedirs(out_dir, exist_ok=True)
        for i in range(frame_count):
            box = (i * height, 0, (i + 1) * height, height)
            frame = img.crop(box)
            frame.save(os.path.join(out_dir, f'frame_{i}.png'))
        print(f'Sliced {filename} into {frame_count} frames.')

print('All done!') 
import os

total = 0
def file_len(fname):
    with open(fname) as f:
        for i, l in enumerate(f):
            pass
    return i + 1

root_dir = "./src/"
file_set = set()

for dir_, _, files in os.walk(root_dir):
    for file_name in files:
        rel_dir = os.path.relpath(dir_, root_dir)
        rel_file = os.path.join(rel_dir, file_name)
        file_set.add(rel_file)

print(len(file_set))

for i in file_set:
    total += file_len(f".\\src\\{i}")

print(total)
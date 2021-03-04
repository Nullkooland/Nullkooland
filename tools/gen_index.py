import json
import io
import os
from argparse import ArgumentParser

PROP_KEYS = [
    "type", "date", "title", "brief", "headerImage", "tags"]


def get_posts(base_dir: str):
    """Get all posts markdown files from the base directory

    Args:
        base_dir (str): Base directory

    Yields:
        str: Path to post markdown file
    """
    for year_dir in os.scandir(base_dir):
        year_dir: os.DirEntry = year_dir
        # Find posts only in 20xx directories
        if not (year_dir.is_dir() and year_dir.name.startswith("20")):
            continue

        for post_dir in os.scandir(year_dir):
            post_dir: os.DirEntry = post_dir
            md_file = f"{post_dir.path}/{post_dir.name}.md"
            # Found post markdown file
            if os.path.isfile(md_file):
                yield md_file


def read_metadata(post_md_file: str):
    """Read the metadata within a post

    Args:
        post_md_file (str): Path to post markdown file

    Returns:
        dict: {post_id: meta} dictionary
    """
    post_meta = {}
    id = os.path.basename(post_md_file)[:-3]
    post_meta["id"] = id

    with io.open(post_md_file, mode="r", encoding="utf-8") as f:
        in_metadata_section = False
        for line in f:
            line: str = line
            # Skip whitespace line
            if line.isspace():
                continue
            # Detect the boundary of metadata section
            if line.startswith("---"):
                if in_metadata_section:
                    break
                else:
                    in_metadata_section = True
                continue

            # Now we can read metadata
            pos_delimiter = line.find(':')
            key = line[:pos_delimiter]
            value = line[pos_delimiter + 2 :].strip("\"\n")
            
            # Must be one of the valid property keys
            if key not in PROP_KEYS:
                continue

            # Interpret post type
            if key == "type":
                type_str = value.lower()
                type_index = 0

                if type_str == "technical":
                    type_index = 0
                elif type_str == "personal":
                    type_index = 1
                elif type_str == "ramblings":
                    type_index = 2

                post_meta[key] = type_index

            elif key == "tags":
                tags = []
                for tag in value.strip("[ ]").split(", "):
                    tag = tag.strip("\"")
                    tags.append(tag)

                post_meta[key] = tags
            else:
                post_meta[key] = value

    return post_meta


def read_tags(post_metas):
    """Create tag -> [posts with the tag] mappings

    Args:
        post_metas (dict): {post_id: meta} dictionary

    Returns:
        dict: {tag: [post_ids]} dictionary
    """

    tags = {}
    for id, meta in post_metas.items():
        for tag in meta["tags"]:
            if tag not in tags:
                tags[tag] = []

            tags[tag].append(id)

    return tags


if __name__ == "__main__":
    """Main function
    """
    # Read args
    parser = ArgumentParser()
    parser.add_argument(
        "-d", "--dir",
        type=str,
        default=".",
        help="Base directory",)
    args = parser.parse_args()

    # Read metadatas from all posts
    post_metas = {}
    for post in get_posts(args.dir):
        meta = read_metadata(post)
        post_metas[meta["id"]] = meta

    print(f"Finished reading {len(post_metas)} posts with metadata")

    # Create tag -> [posts with tag] mapping
    tags = read_tags(post_metas)

    print(f"Finished reading {len(tags)} tags")

    # Output json files
    with io.open(f"{args.dir}/index.json", mode="w", encoding="utf-8") as f:
        json.dump(post_metas, f, ensure_ascii=False, indent=4)

    print(f"Finished writing index.json")

    with io.open(f"{args.dir}/tags.json", mode="w", encoding="utf-8") as f:
        json.dump(tags, f, ensure_ascii=False, indent=4)

    print(f"Finished writing tags.json")

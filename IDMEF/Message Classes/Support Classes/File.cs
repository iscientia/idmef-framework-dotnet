using System;
using System.Xml;

namespace idmef
{
	public class File
	{
		private readonly FileCategoryEnum category = FileCategoryEnum.unknown;
		private readonly string ident = "0";
		private readonly string name;
		private readonly string path;
		public DateTime? accessTime;
		public Checksum[] checksum;
		public DateTime? createTime;
		public Int64? dataSize;
		public Int64? diskSize;
		public FileAccess[] fileAccess;
		public string fileType;
		public FileSystemTypeEnum fstype = FileSystemTypeEnum.unknown;
		public Inode inode;
		public Linkage[] linkage;
		public DateTime? modifyTime;

		public File(string name, string path, FileCategoryEnum category)
		{
			if (string.IsNullOrEmpty(name)) throw new ArgumentException("File must have a name node.");
			if (string.IsNullOrEmpty(path)) throw new ArgumentException("File must have a path node.");
			if (category == FileCategoryEnum.unknown) throw new ArgumentException("File must have a category attribute.");
			this.name = name;
			this.path = path;
			this.category = category;
		}

		public File(string name, string path, DateTime? createTime, DateTime? modifyTime, DateTime? accessTime,
		            Int64? dataSize, Int64? diskSize, FileAccess[] fileAccess, Linkage[] linkage, Inode inode,
		            Checksum[] checksum, FileCategoryEnum category)
			: this(name, path, category)
		{
			this.createTime = createTime;
			this.modifyTime = modifyTime;
			this.accessTime = accessTime;
			this.dataSize = dataSize;
			this.diskSize = diskSize;
			this.fileAccess = fileAccess;
			this.linkage = linkage;
			this.inode = inode;
			this.checksum = checksum;
		}

		public File(string name, string path, DateTime? createTime, DateTime? modifyTime, DateTime? accessTime,
		            Int64? dataSize, Int64? diskSize, FileAccess[] fileAccess, Linkage[] linkage, Inode inode,
		            Checksum[] checksum, string ident, FileCategoryEnum category, FileSystemTypeEnum type,
		            string fileType)
			: this(name, path, createTime, modifyTime, accessTime, dataSize, diskSize, fileAccess, linkage, inode,
			       checksum, category)
		{
			this.ident = string.IsNullOrEmpty(ident) ? "0" : ident;
			fstype = type;
			this.fileType = fileType;
		}


		public XmlElement ToXml(XmlDocument document)
		{
			XmlElement fileNode = document.CreateElement("idmef:File", "http://iana.org/idmef");

			fileNode.SetAttribute("ident", ident);
			fileNode.SetAttribute("category", EnumDescription.GetEnumDescription(category));
			if (fstype != FileSystemTypeEnum.unknown) fileNode.SetAttribute("fstype", EnumDescription.GetEnumDescription(fstype));
			if (!string.IsNullOrEmpty(fileType)) fileNode.SetAttribute("file-type", fileType);

			XmlElement fileSubNode = document.CreateElement("idmef:name", "http://iana.org/idmef");
			XmlNode subNode = document.CreateNode(XmlNodeType.Text, "idmef", "name", "http://iana.org/idmef");
			subNode.Value = name;
			fileSubNode.AppendChild(subNode);
			fileNode.AppendChild(fileSubNode);
			fileSubNode = document.CreateElement("idmef:path", "http://iana.org/idmef");
			subNode = document.CreateNode(XmlNodeType.Text, "idmef", "path", "http://iana.org/idmef");
			subNode.Value = path;
			fileSubNode.AppendChild(subNode);
			fileNode.AppendChild(fileSubNode);
			if (createTime != null)
			{
				fileSubNode = document.CreateElement("idmef:create-time", "http://iana.org/idmef");
				subNode = document.CreateNode(XmlNodeType.Text, "idmef", "create-time", "http://iana.org/idmef");
				subNode.Value = ((DateTime)createTime).ToString("o");
				fileSubNode.AppendChild(subNode);
				fileNode.AppendChild(fileSubNode);
			}
			if (modifyTime != null)
			{
				fileSubNode = document.CreateElement("idmef:modify-time", "http://iana.org/idmef");
				subNode = document.CreateNode(XmlNodeType.Text, "idmef", "modify-time", "http://iana.org/idmef");
				subNode.Value = ((DateTime)modifyTime).ToString("o");
				fileSubNode.AppendChild(subNode);
				fileNode.AppendChild(fileSubNode);
			}
			if (accessTime != null)
			{
				fileSubNode = document.CreateElement("idmef:access-time", "http://iana.org/idmef");
				subNode = document.CreateNode(XmlNodeType.Text, "idmef", "access-time", "http://iana.org/idmef");
				subNode.Value = ((DateTime)accessTime).ToString("o");
				fileSubNode.AppendChild(subNode);
				fileNode.AppendChild(fileSubNode);
			}
			if (dataSize != null)
			{
				fileSubNode = document.CreateElement("idmef:data-size", "http://iana.org/idmef");
				subNode = document.CreateNode(XmlNodeType.Text, "idmef", "data-size", "http://iana.org/idmef");
				subNode.Value = dataSize.ToString();
				fileSubNode.AppendChild(subNode);
				fileNode.AppendChild(fileSubNode);
			}
			if (diskSize != null)
			{
				fileSubNode = document.CreateElement("idmef:disk-size", "http://iana.org/idmef");
				subNode = document.CreateNode(XmlNodeType.Text, "idmef", "disk-size", "http://iana.org/idmef");
				subNode.Value = diskSize.ToString();
				fileSubNode.AppendChild(subNode);
				fileNode.AppendChild(fileSubNode);
			}
			if ((fileAccess != null) && (fileAccess.Length > 0))
				foreach (var fa in fileAccess) if (fa != null) fileNode.AppendChild(fa.ToXml(document));
			if ((linkage != null) && (linkage.Length > 0))
				foreach (var lnk in linkage) if (lnk != null) fileNode.AppendChild(lnk.ToXml(document));
			if (inode != null) fileNode.AppendChild(inode.ToXml(document));
			if ((checksum != null) && (checksum.Length > 0))
				foreach (var chs in checksum) if (chs != null) fileNode.AppendChild(chs.ToXml(document));

			return fileNode;
		}
	}
}
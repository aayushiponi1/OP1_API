using Microsoft.AspNetCore.Mvc;
using System.Net;
using OP1_API.Areas.UserService.Models;
using OP1_API.Models;



namespace OP1_API.ClassCollection
{
    public class DocumentUpload
    {
        GlobalClass cl;
        public DocumentUpload(GlobalClass _cl)
        {
            cl = _cl;
        }
        public ReturnResult uploadDocuments(uploadDocumentsParam p)
        {
            ReturnResult ret = new ReturnResult();
            try
            {
                if (p.file == null || p.file.Length == 0)
                {
                    ret.result = "FAILED";
                    ret.retval = "0";
                    ret.msg = "No File uploaded !";
                }
                else
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".xls", ".xlsx" };
                    var fileExtension = Path.GetExtension(p.file.FileName).ToLower();
                    if (!Array.Exists(allowedExtensions, ext => ext == fileExtension))
                    {
                        ret.result = "FAILED";
                        ret.retval = "0";
                        ret.msg = "Invalid File Type, Only upload [" + allowedExtensions + "] !";
                    }
                    else
                    {
                        string _uploadFolder = cl.postImageUrl() + p.rootPath;
                        if (!Directory.Exists(_uploadFolder))
                        {
                            Directory.CreateDirectory(_uploadFolder);
                        }
                        var flName = Guid.NewGuid().ToString() + p.file.FileName;
                        var filePath = Path.Combine(_uploadFolder, flName);

                        // Save the file to the server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            p.file.CopyTo(fileStream);
                        }

                        ret.result = "OK";
                        ret.retval = flName;
                        ret.msg = "Document Upload Successfull !";
                    }

                }
            }
            catch (Exception ex)
            {
                ret.result = "FAILED";
                ret.retval = "0";
                ret.msg = "Document Not uploaded !";
            }
            return ret;
        }

        public List<ReturnResult> uploadmultipleDocuments(uploadmultipleDocumentsParam p)
        {
            List<ReturnResult> ret = new List<ReturnResult>();
            try
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".xls", ".xlsx" };

                if (p.rootPath.Trim() == "")
                {
                    ret.Add(new ReturnResult
                    {
                        result = "FAILED",
                        retval = "0",
                        msg = "Please Add Directory Path !"
                    });
                }
                else if (p.files == null || p.files.Count == 0)
                {
                    ret.Add(new ReturnResult
                    {
                        result = "FAILED",
                        retval = "0",
                        msg = "Please Select Document for Upload !"
                    });
                }
                else
                {

                    foreach (var file in p.files)
                    {
                        var fileExtension = Path.GetExtension(file.FileName).ToLower();
                        if (!Array.Exists(allowedExtensions, ext => ext == fileExtension))
                        {
                            ret.Add(new ReturnResult
                            {
                                result = "FAILED",
                                retval = fileExtension,
                                msg = "Invalid File Type, Only allowed [" + allowedExtensions + "] !"
                            });
                        }
                        string _uploadFolder = cl.postImageUrl() + p.rootPath;
                        if (!Directory.Exists(_uploadFolder))
                        {
                            Directory.CreateDirectory(_uploadFolder);
                        }
                        var flName = Guid.NewGuid().ToString() + file.FileName;
                        var filePath = Path.Combine(_uploadFolder, flName);
                        // Save the file to the server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        ret.Add(new ReturnResult
                        {
                            result = "OK",
                            retval = flName,
                            msg = "Document Uploaded !"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ret.Add(new ReturnResult
                {
                    result = "FAILED",
                    retval = "0",
                    msg = "Document Not uploaded !"
                });
            }
            return ret;
        }

        public ReturnResult UploadpaymentDoc(uploadDocumentsParam p)
        {
            ReturnResult ret = new ReturnResult();

            try
            {
                if (p.file == null || p.file.Length == 0)
                {
                    ret.result = "FAILED";
                    ret.retval = "0";
                    ret.msg = "No file uploaded!";
                }
                else
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".xls", ".xlsx" };
                    var extension = Path.GetExtension(p.file.FileName).ToLower();

                    if (!Array.Exists(allowedExtensions, ext => ext == extension))
                    {
                        ret.result = "FAILED";
                        ret.retval = "0";
                        ret.msg = "Invalid file type. Allowed types: " + string.Join(", ", allowedExtensions);
                    }
                    else
                    {
                        // Generate encrypted filename
                        string encryptedFileName = GenerateEncryptedFileName(extension);

                        // Use rootPath if provided, else default path
                        string basePath = !string.IsNullOrWhiteSpace(p.rootPath)
                            ? p.rootPath
                            : @"C:\Website\Bharat\Admin\OP1\PaymentDoc";

                        string savePath = Path.Combine(basePath, encryptedFileName);

                        // Ensure directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                        // Save the file
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            p.file.CopyTo(stream);
                        }

                        // Build public URL (assuming rootPath maps to /Bilty_Docs/)
                        string fileUrl = $"https://bharat.we1.tech/Admin/OP1/PaymentDoc/{encryptedFileName}";

                        ret.result = "OK";
                        ret.retval = encryptedFileName;
                        ret.msg = "Document uploaded successfully!";
                    }
                }
            }
            catch (Exception ex)
            {
                ret.result = "FAILED";
                ret.retval = "0";
                ret.msg = "Document not uploaded! " + ex.Message;
            }

            return ret;
        }



        private string GenerateEncryptedFileName(string extension)
            {
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string randomPart = Guid.NewGuid().ToString("N").Substring(0, 6);
                return $"{timestamp}_{randomPart}{extension}";
            }
        }

    



}

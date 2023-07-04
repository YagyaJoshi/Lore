using Loregroup.Core.Enumerations;
using Loregroup.Core.Exceptions;
using Loregroup.Core.Interfaces.Providers;
using Loregroup.Core.Interfaces.Utilities;
using Loregroup.Core.ViewModels;
using Loregroup.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loregroup.Data.Entities;

namespace Loregroup.Provider
{
    public class ContentProvider : IContentProvider {
        private readonly AppContext _context;
        private readonly ISecurity _security;
        private readonly IUtilities _utilities;
        private readonly IConfigSettingProvider _configProvider;
        private readonly ISession _session;

        public ContentProvider(AppContext context, ISecurity security, IUtilities utilities, IConfigSettingProvider configProvider, ISession session)
        {
            _context = context;
            _security = security;
            _utilities = utilities;
            _configProvider = configProvider;
            _session = session;
        }


        //manisha
        //public FileViewModel ToFileViewModel(Data.Entities.FileLibrary file)
        //{
        //    return new FileViewModel()
        //    {
        //        Id = file.Id,
        //        FileUrl =file.FilePath, //"/Content/File/" + file.Id+"/"+file.FileName,
        //        FileType = (FileType) file.FileType,
        //        ThumbnailImageUrl = file.ThumbnailPath,
        //        ThumbType = (FileType) file.ThumbnailFileType,
        //    };
        //}

        //public FileDetailViewModel ToFileDetailViewModel(Data.Entities.FileLibrary file)
        //{
        //    return new FileDetailViewModel()
        //    {
        //        Id = file.Id,
        //        FileName = file.FileName,
        //        FileType = (FileType)file.FileType,
        //        ThumbnailName = file.ThumbnailName,
        //        ThumbnailFileType = (FileType)file.ThumbnailFileType,
        //        FilePath = file.FilePath,
        //        FileRelation = (FileRelation)file.FileRelation,
        //        IsInFileSystem = file.IsInFileSystem,
        //        ThumbnailPath = file.ThumbnailPath,
        //        Status = (Status)file.StatusId,
        //        ModifiedById = file.ModifiedById,
        //        ModificationDate = file.ModificationDate,
        //        CreatedById = file.CreatedById,
        //        CreationDate = file.CreationDate
        //    };
        //}

        //public Int64 SaveFilelibrary(String filename, String filepath)
        //{
        //    try
        //    {
        //        FileLibrary file = new FileLibrary()
        //        {
        //            IsInFileSystem = true,
        //            FileName = filename,
        //            FilePath = filepath,
        //            FileRelation = 1,
        //            StatusId = (int)Status.Active,
        //            FileType = 1,
        //            ThumbnailFileType = 1,
        //            ThumbnailName = filename,
        //            ThumbnailPath = filepath,
        //            CreatedById = (int)_session.CurrentUser.Id,
        //            ModifiedById = (int)_session.CurrentUser.Id
        //        };
        //        _context.FileLibrary.Add(file);
        //        _context.SaveChanges();
        //        return file.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public Int64 SaveLogoFilelibrary(String headerfilename, String headerfilepath, String footerfilename, String footerfilepath)
        //{
        //    try
        //    {
        //        FileLibrary newFile = new FileLibrary()
        //        {
        //            IsInFileSystem = true,
        //            FileName = headerfilename,
        //            FilePath = headerfilepath,
        //            FileRelation = 1,
        //            StatusId = (int)Status.Active,
        //            FileType = 1,
        //            ThumbnailName = footerfilename,
        //            ThumbnailPath = footerfilepath,
        //            CreatedById = (int)_session.CurrentUser.Id,
        //            ModifiedById = (int)_session.CurrentUser.Id
        //        };
        //        _context.FileLibrary.Add(newFile);
        //        _context.SaveChanges();
        //        return newFile.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public Int64 SaveSignatureImage(String SignatureImage,String SignatureImagePath)
        //{
        //    try {
        //        FileLibrary newsignFile = new FileLibrary()
        //        {
        //            IsInFileSystem = true,
        //            FileName = SignatureImage,
        //            FilePath = SignatureImagePath,
        //            FileRelation = 1,
        //            StatusId = (int)Status.Active,
        //            FileType = 1,
        //            ThumbnailName = SignatureImage,
        //            ThumbnailPath = SignatureImagePath,
        //            CreatedById = (int)_session.CurrentUser.Id,
        //            ModifiedById = (int)_session.CurrentUser.Id

        //        };
        //        _context.FileLibrary.Add(newsignFile);
        //        _context.SaveChanges();
        //        return newsignFile.Id;
        //    }
        //    catch(Exception ex) {
        //        throw ex;
        //    }
 
        //}

        //public FileDetailViewModel GetFile(Int64 id)
        //{
        //    FileLibrary file = _context.FileLibrary.FirstOrDefault(x => x.Id == id);
        //    if (file != null)
        //    {
        //        return ToFileDetailViewModel(file);
        //    }
        //    throw new ContentNotFoundException();
        //}

        //public Int64 GetCurrentId() {
        //    return _context.FileLibrary.Count() + 1;
        //}
    }
}
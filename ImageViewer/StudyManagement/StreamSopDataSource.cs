﻿#region License

// Copyright (c) 2013, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This file is part of the ClearCanvas RIS/PACS open source project.
//
// The ClearCanvas RIS/PACS open source project is free software: you can
// redistribute it and/or modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// The ClearCanvas RIS/PACS open source project is distributed in the hope that it
// will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General
// Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// the ClearCanvas RIS/PACS open source project.  If not, see
// <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.IO;
using ClearCanvas.Dicom;

namespace ClearCanvas.ImageViewer.StudyManagement
{
	/// <summary>
	/// An <see cref="ISopDataSource"/> whose underlying data resides in a re-openable
	/// <see cref="Stream">stream</see>.
	/// </summary>
	/// <remarks>
	/// The stream must be re-openable and always readable and seekable, as pixel
	/// data is always loaded on-demand, just like in <see cref="LocalSopDataSource"/>,
	/// which is an otherwise identical implementation.
	/// </remarks>
	public class StreamSopDataSource : DicomMessageSopDataSource
	{
		private readonly Func<Stream> _streamOpener;

		public StreamSopDataSource(Func<Stream> streamOpener)
			: base(new DicomFile())
		{
			_streamOpener = streamOpener;
		}

		public DicomFile File
		{
			get { return (DicomFile)GetSourceMessage(true); }
		}

		protected override void EnsureLoaded()
		{
			File.Load(_streamOpener, null, DicomReadOptions.Default | DicomReadOptions.StorePixelDataReferences);
		}
	}
}
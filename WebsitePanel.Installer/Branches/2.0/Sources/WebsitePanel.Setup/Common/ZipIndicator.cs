// Copyright (c) 2010, SMB SAAS Systems Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

using Ionic.Zip;

namespace WebsitePanel.Setup.Common
{
	/// <summary>
	/// Shows progress of file zipping.
	/// </summary>
	public sealed class ZipIndicator
	{
		private ProgressBar progressBar;
		string sourcePath;
		string zipFile;
		int totalFiles = 0;
		int files = 0;

		public ZipIndicator(ProgressBar progressBar, string sourcePath, string zipFile)
		{
			this.progressBar = progressBar;
			this.sourcePath = sourcePath;
			this.zipFile = zipFile;
		}

		public void Start()
		{
			totalFiles = FileUtils.CalculateFiles(sourcePath);
			using (ZipFile zip = new ZipFile())
			{
				zip.AddProgress += ShowProgress;
				zip.UseUnicodeAsNecessary = true;
				zip.AddDirectory(sourcePath);
				zip.Save(zipFile);
			}
		}

		private void ShowProgress(object sender, AddProgressEventArgs e)
		{
			if (e.EventType == ZipProgressEventType.Adding_AfterAddEntry)
			{
				string fileName = e.CurrentEntry.FileName;
				files++;
				this.progressBar.Value = Convert.ToInt32(files * 100 / totalFiles);
				this.progressBar.Update();
			}
		}
	}
}

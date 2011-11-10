using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace EWSProject_Commentary.Commentary
{
    public partial class CommentaryUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateProposalsList();
            }

            PopulateProposalsTable();
            PopulateInterestedTable();  
        }
            
        //my methods

        public void PopulateProposalsTableAll()
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Proposals"];

            //add header rows
            TableHeaderRow thr = new TableHeaderRow();
            TableHeaderCell thrTitle = new TableHeaderCell();
            TableHeaderCell thrDesc = new TableHeaderCell(  );
            thr.Cells.Add(thrTitle);
            thr.Cells.Add(thrDesc);
            proposalsTable.Rows.Add(thr);
            thrTitle.Text = "Title";
            thrDesc.Text = "Description";

            //start adding every single proposal into the proposal list view
            foreach (SPListItem listItem in objList.Items)
            {
                TableRow row = new TableRow();
                TableCell titleCell = new TableCell();
                TableCell descCell = new TableCell();   
                row.Cells.Add(titleCell);
                row.Cells.Add(descCell);
                proposalsTable.Rows.Add(row);

                HyperLink titleLink = new HyperLink();
                titleLink.Text = listItem["Title"].ToString();
                titleLink.NavigateUrl = listItem[SPBuiltInFieldId.EncodedAbsUrl].ToString();
                titleCell.Controls.Add(titleLink);

                if (listItem["Description"] != null)
                {
                    descCell.Text = listItem["Description"].ToString();
                }
                else
                {
                    descCell.Text = "nil";
                }


            } //for each
        }

        public void PopulateProposalsTable()
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Proposals"];

            int displayNoOfItems = 5;
            int totalNoOfItems = objList.Items.Count;

            proposalsTable.Width = 280;

            //if there are lesser than displayNoOfItems, just show all
            if (totalNoOfItems <= displayNoOfItems)
            {
                PopulateProposalsTableAll();
                return;
            }

            //add header rows
            TableHeaderRow thr = new TableHeaderRow();
            TableHeaderCell thrTitle = new TableHeaderCell();
            TableHeaderCell thrDesc = new TableHeaderCell();
            thr.Cells.Add(thrTitle);
            thr.Cells.Add(thrDesc);
            proposalsTable.Rows.Add(thr);
            thrTitle.Text = "Title";
            thrDesc.Text = "Description";

            //create an array and init with values
            int[] randomArray = new int[totalNoOfItems];
            for (int i = 0; i < randomArray.Length; i++)
            {
                randomArray[i] = i;
            }

            //shuffle the array
            var rand = new Random();
            for (int i = randomArray.Length - 1; i > 0; i--)
            {
                int n = rand.Next(i + 1);
                int temp = randomArray[i];
                randomArray[i] = randomArray[n];
                randomArray[n] = temp;
            }

            for (int i = 0; i < displayNoOfItems; i++)
            {
                //retrieve the item
                SPListItem listItem = objList.Items[randomArray[i]];

                TableRow row = new TableRow();
                TableCell titleCell = new TableCell();
                TableCell descCell = new TableCell();
                row.Cells.Add(titleCell);
                row.Cells.Add(descCell);
                proposalsTable.Rows.Add(row);

                HyperLink titleLink = new HyperLink();
                titleLink.Text = listItem["Title"].ToString();
                titleLink.NavigateUrl = listItem[SPBuiltInFieldId.EncodedAbsUrl].ToString();
                titleCell.Controls.Add(titleLink);

                if (listItem["Description"] != null)
                {
                    descCell.Text = listItem["Description"].ToString();
                }
                else
                {
                    descCell.Text = "nil";
                }

            }
        }

        public void PopulateProposalsList()
        {
            proposalsList.Width = 250;

            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Proposals"];

            proposalsList.Items.Add(new ListItem("--SELECT--", "0"));

            //start adding every single proposal into the proposal list view
            foreach (SPListItem listItem in objList.Items)
            {
                ListItem l = new ListItem(listItem["Title"].ToString(), listItem["Proposal ID"].ToString());
                proposalsList.Items.Add(l);        
            }
        }

        public void PopulateProposalInfoLabel()
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Proposals"];

            //for for the specific proposal
            foreach (SPListItem listItem in objList.Items)
            {
                if (Convert.ToInt32(listItem["Proposal ID"]) == Convert.ToInt32(proposalsList.SelectedItem.Value))
                {
                    proposalInfoLabel.Text = "<strong>You have selected:</strong></br>";
                    proposalInfoLabel.Text += "<i>Title</i>: " + listItem["Title"] + "<br/>";
                    

                    if (listItem["Description"] == null)
                    {
                        proposalInfoLabel.Text += "<i>Description</i>: nil<br/>";
                    } else {
                        proposalInfoLabel.Text += "<i>Description</i>: " + listItem["Description"] + "<br/>";
                    }

                    proposalInfoLabel.Text += "<i>Keyword(s)</i>: " + listItem["Keyword"] + "<br/>";
                    break;
                }
            }
        }

        public void PopulateCommentsTable()
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Comments"];

            commentsTable.Width = 280;

            bool isEmpty = true;

            //remove all rows
            commentsTable.Rows.Clear();

            //start populating all the comments
            foreach (SPListItem listItem in objList.Items)
            {
                if (Convert.ToInt32(listItem["Proposal ID"]) == Convert.ToInt32(proposalsList.SelectedItem.Value))
                {
                    TableRow tr = new TableRow();
                    TableCell tc = new TableCell();
                    tr.Cells.Add(tc);
                    commentsTable.Rows.Add(tr);

                    tc.Text = "<b>" + listItem["User ID"].ToString()
                        + "</b>&nbsp;&nbsp;" + listItem["Comment"].ToString()
                        + "</br>Posted on: " + listItem[SPBuiltInFieldId.Created_x0020_Date].ToString();
                  
                    isEmpty = false;
                }
            }

            if (isEmpty)
            {
                TableRow tr = new TableRow();
                TableCell tc = new TableCell();
                tr.Cells.Add(tc);
                commentsTable.Rows.Add(tr);

                tc.Text = "No comments";
            }

        }

        public void PopulateCommentsAndInterestLabel(int proposalID)
        {
            noOfCommentLikesLabel.Text = GetLikesOrDislikesCount(proposalID, "Like").ToString();
            noOfCommentDislikesLabel.Text = GetLikesOrDislikesCount(proposalID, "Dislike").ToString();
        }

        public void DecideLikeDislikeEnable(int proposalID)
        {
    

            if (HasLikeOrDislike(proposalID, "Like"))
            {
               
                //if there is a Like from the user
                likeCommentButton.Enabled = false;
                dislikeCommentButton.Enabled = true;
            }
            else if (HasLikeOrDislike(proposalID, "Dislike"))
            {
  
                //if there is a Dislike from the user
                likeCommentButton.Enabled = true;
                dislikeCommentButton.Enabled = false;
            }
            else
            {
             
                //neither like or dislike
                likeCommentButton.Enabled = true;
                dislikeCommentButton.Enabled = true;
            }
        }

        public void DecideFlagEnabledOrDisabled(int proposalID)
        {
            if (this.HasFlag(proposalID))
            {
                indicateFlagLabelInterestedNotInterested.Visible = true;
                indicateFlagLabelInterested.Visible = false;
            }
            else
            {
                indicateFlagLabelInterestedNotInterested.Visible = false;
                indicateFlagLabelInterested.Visible = true;
            }
        }

        public int AddComment(int proposalID, String comment)
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Comments"];

            SPListItem newListItem = objList.Items.Add();
            newListItem["Proposal ID"] = proposalID.ToString();
            newListItem["User ID"] = GetCurrentUserWithDomain();
            newListItem["Comment"] = comment;
            newListItem.Update();

            return Convert.ToInt32(newListItem["ID"]);
        }

        //need to include validation, check if the person had prev liked for the same proposal or dislike
        public void AddLikeOrDislike(int proposalID, String likeOrDislike)
        {
            //if the user is trying to Like, and he has Liked before, return
            if (HasLikeOrDislike(proposalID, "Like") && likeOrDislike == "Like")
            {
                return;
            }

            //if the user is trying to Dislike, and he has Disliked before, return
            if (HasLikeOrDislike(proposalID, "Dislike") && likeOrDislike == "Dislike")
            {
                return;
            }

            if (HasLikeOrDislike(proposalID, "Like") && likeOrDislike == "Dislike")
            {
                //if the user is trying to Dislike, but he has a like
                RemoveLikeOrDislike(proposalID, "Like");
            }
            else if (HasLikeOrDislike(proposalID, "Dislike") && likeOrDislike == "Like")
            {
                //if the user is trying to Like, but he has a Dislike
                RemoveLikeOrDislike(proposalID, "Dislike");
            }
            
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Likes"];

            SPListItem newListItem = objList.Items.Add();
            newListItem["User ID"] = GetCurrentUserWithDomain();
            newListItem["Proposal ID"] = proposalID.ToString();
            newListItem["Like"] = likeOrDislike;
            newListItem.Update();
        }

        public bool RemoveLikeOrDislike(int proposalID, String likeOrDislike)
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Likes"];

            bool hasRemoved = false;

            foreach (SPListItem listItem in objList.Items)
            {
                if (Convert.ToInt32(listItem["Proposal ID"].ToString()) == proposalID
                    && likeOrDislike == listItem["Like"].ToString()
                    && GetCurrentUserWithDomain().ToUpper() == listItem["User ID"].ToString().ToUpper())
                {
                    listItem.Delete();
                    hasRemoved = true;
                    break;
                }
            }

            return hasRemoved;
        }

        public String GetCurrentUserWithDomain()
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            return objCurrentWeb.CurrentUser.LoginName.Substring(objCurrentWeb.CurrentUser.LoginName.IndexOf('\\') + 1);
        }

        public int GetLikesOrDislikesCount(int proposalID, String likeOrDislike)
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Likes"];

            int count = 0;

            foreach (SPListItem listItem in objList.Items)
            {
                if (Convert.ToInt32(listItem["Proposal ID"]) == proposalID
                    && likeOrDislike == listItem["Like"].ToString())
                {
                    count++;
                }
            }

            return count;
        }

        public int GetFlagCount(int proposalID)
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Flags"];

            int count = 0;

            foreach (SPListItem listItem in objList.Items)
            {
                if (Convert.ToInt32(listItem["Proposal ID"]) == proposalID)
                {
                    count++;
                }
            }

            return count;
        }

        public bool HasLikeOrDislike(int proposalID, String likeOrDislike)
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Likes"];

            foreach (SPListItem listItem in objList.Items)
            {
                if (Convert.ToInt32(listItem["Proposal ID"].ToString()) == proposalID
                    && likeOrDislike == listItem["Like"].ToString() 
                    && GetCurrentUserWithDomain().ToUpper() == listItem["User ID"].ToString().ToUpper())
                {
                    return true;
                }
            }

            return false;
        }

        public void AddFlag(int proposalID)
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Flags"];

            SPListItem newListItem = objList.Items.Add();
            newListItem["User ID"] = GetCurrentUserWithDomain();
            newListItem["Proposal ID"] = proposalID.ToString();
            newListItem.Update();
        }

        public bool RemoveFlag(int proposalID)
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Flags"];

            bool hasRemoved = false;

            foreach (SPListItem listItem in objList.Items)
            {
                if (Convert.ToInt32(listItem["Proposal ID"].ToString()) == proposalID
                    && GetCurrentUserWithDomain().ToUpper() == listItem["User ID"].ToString().ToUpper())
                {
                    listItem.Delete();
                    hasRemoved = true;
                    break;
                }
            }

            return hasRemoved;
        }

        public bool HasFlag(int proposalID)
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Flags"];

            foreach (SPListItem listItem in objList.Items)
            {
                if (Convert.ToInt32(listItem["Proposal ID"].ToString()) == proposalID
                    && GetCurrentUserWithDomain().ToUpper() == listItem["User ID"].ToString().ToUpper())
                {
                    return true;
                }
            }

            return false;
        }

        public void PopulateInterestedTable()
        {
            int count = 0;

            interestedProposalsTable.Width = 280;

            TableHeaderRow headerRow = new TableHeaderRow();
            TableHeaderCell titleCell = new TableHeaderCell();
            TableHeaderCell likeNoCell = new TableHeaderCell();
            TableHeaderCell disLikeNoCell = new TableHeaderCell();
            TableHeaderCell interestNoCell = new TableHeaderCell();

            headerRow.Cells.Add(titleCell);
            headerRow.Cells.Add(likeNoCell);
            headerRow.Cells.Add(disLikeNoCell);
            headerRow.Cells.Add(interestNoCell);
            titleCell.Text = "Title";
            likeNoCell.Text = "Likes";
            disLikeNoCell.Text = "Dislikes";
            interestNoCell.Text = "Interests";
            interestedProposalsTable.Rows.Add(headerRow);

            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Flags"];

            foreach (SPListItem listItem in objList.Items)
            {
                if (GetCurrentUserWithDomain().ToUpper() == listItem["User ID"].ToString().ToUpper())
                {
                    count++;

                    string[] data = RetrieveProposalInfo(Convert.ToInt32(listItem["Proposal ID"].ToString()));

                    TableRow row = new TableRow();
                    TableCell titleCellValue = new TableCell();
                    TableCell likeNoCellValue = new TableCell();
                    TableCell disLikeNoCellValue = new TableCell();
                    TableCell interestNoCellValue = new TableCell();
                    
                    //set horizontal alignment
                    titleCellValue.HorizontalAlign = HorizontalAlign.Center;
                    likeNoCellValue.HorizontalAlign = HorizontalAlign.Center;
                    disLikeNoCellValue.HorizontalAlign = HorizontalAlign.Center;
                    interestNoCellValue.HorizontalAlign = HorizontalAlign.Center;

                    row.Cells.Add(titleCellValue);
                    row.Cells.Add(likeNoCellValue);
                    row.Cells.Add(disLikeNoCellValue);
                    row.Cells.Add(interestNoCellValue);
                    titleCellValue.Text = data[0];
                    likeNoCellValue.Text = data[1];
                    disLikeNoCellValue.Text = data[2];
                    interestNoCellValue.Text = data[3];
                    interestedProposalsTable.Rows.Add(row);
                }
            }

            if (count == 0)
            {
                TableCell forEmpty = new TableCell();
                forEmpty.ColumnSpan = 4;
                forEmpty.Text = "you have no interested proposals";
                forEmpty.HorizontalAlign = HorizontalAlign.Center;
                TableRow tr = new TableRow();
                tr.Cells.Add(forEmpty);
                interestedProposalsTable.Rows.Add(tr);
            }
        }

        public string[] RetrieveProposalInfo(int proposalID)
        {
            SPWeb objCurrentWeb = SPContext.Current.Web;
            SPList objList = objCurrentWeb.Lists["Proposals"];

            string[] proposalInfos = new string[5];

            foreach (SPListItem listItem in objList.Items)
            {
                
                if (proposalID == Convert.ToInt32(listItem["Proposal ID"]))
                {
                    proposalInfos[0] = listItem["Title"].ToString();
                    proposalInfos[1] = GetLikesOrDislikesCount(proposalID, "Like").ToString();
                    proposalInfos[2] = GetLikesOrDislikesCount(proposalID, "Dislike").ToString();
                    proposalInfos[3] = GetFlagCount(proposalID).ToString();
                    //proposalInfos[4] = listItem["Status"].ToString();
                    proposalInfos[4] = "for status";

                    break;
                }
            }

            return proposalInfos;
        }


        //events

        protected void indicateFlagLabelInterested_Click(object sender, EventArgs e)
        {
            AddFlag(Convert.ToInt32(currentPoposalID.Value));
                
            PopulateCommentsTable();
            PopulateCommentsAndInterestLabel(Convert.ToInt32(proposalsList.SelectedItem.Value));
            DecideFlagEnabledOrDisabled(Convert.ToInt32(proposalsList.SelectedItem.Value));
            DecideLikeDislikeEnable(Convert.ToInt32(proposalsList.SelectedItem.Value));

            interestedProposalsTable.Rows.Clear();
            PopulateInterestedTable();
        }

        protected void indicateFlagLabelInterestedNotInterested_Click(object sender, EventArgs e)
        {
            RemoveFlag(Convert.ToInt32(currentPoposalID.Value));

            PopulateCommentsTable();
            PopulateCommentsAndInterestLabel(Convert.ToInt32(proposalsList.SelectedItem.Value));
            DecideFlagEnabledOrDisabled(Convert.ToInt32(proposalsList.SelectedItem.Value));
            DecideLikeDislikeEnable(Convert.ToInt32(proposalsList.SelectedItem.Value));

            interestedProposalsTable.Rows.Clear();
            PopulateInterestedTable();
        }

        protected void likeCommentButton_Click(object sender, EventArgs e)
        {
            AddLikeOrDislike(Convert.ToInt32(proposalsList.SelectedItem.Value), "Like");
            PopulateCommentsTable();
            PopulateCommentsAndInterestLabel(Convert.ToInt32(proposalsList.SelectedItem.Value));
            DecideFlagEnabledOrDisabled(Convert.ToInt32(proposalsList.SelectedItem.Value));
            DecideLikeDislikeEnable(Convert.ToInt32(proposalsList.SelectedItem.Value));
        }

        protected void dislikeCommentButton_Click(object sender, EventArgs e)
        {
            AddLikeOrDislike(Convert.ToInt32(proposalsList.SelectedItem.Value), "Dislike");
            PopulateCommentsTable();
            PopulateCommentsAndInterestLabel(Convert.ToInt32(proposalsList.SelectedItem.Value));
            DecideFlagEnabledOrDisabled(Convert.ToInt32(proposalsList.SelectedItem.Value));
            DecideLikeDislikeEnable(Convert.ToInt32(proposalsList.SelectedItem.Value));
        }

        protected void addCommentButton_Click(object sender, EventArgs e)
        {
            AddComment(Convert.ToInt32(currentPoposalID.Value), commentTextBox.Text);
            PopulateCommentsTable(); //reprint the table
            commentTextBox.Text = ""; //clear the textbox   
        }

        protected void proposalsList_SelectedIndexChanged1(object sender, EventArgs e)
        {
            currentPoposalID.Value = proposalsList.SelectedItem.Value;

            //if no proposal is selected
            if (proposalsList.SelectedItem.Value.ToString() == "0")
            {
                //make all the controls visible
                likeCommentButton.Visible = false;
                dislikeCommentButton.Visible = false;
                likeLabel.Visible = false;
                noOfCommentLikesLabel.Visible = false;
                dislikeLabel.Visible = false;
                noOfCommentDislikesLabel.Visible = false;
                commentsLabel.Visible = false;
                addCommentButton.Visible = false;
                commentTextBox.Visible = false;
                indicateFlagLabelInterested.Visible = false;
                indicateFlagLabelInterestedNotInterested.Visible = false;
                proposalInfoLabel.Visible = false;
                return;
            }

            PopulateCommentsTable();
            PopulateProposalInfoLabel();

            //make all the controls visible
            likeCommentButton.Visible = true;
            dislikeCommentButton.Visible = true;
            likeLabel.Visible = true;
            noOfCommentLikesLabel.Visible = true;
            dislikeLabel.Visible = true;
            noOfCommentDislikesLabel.Visible = true;
            commentsLabel.Visible = true;
            addCommentButton.Visible = true;
            commentTextBox.Visible = true;
            indicateFlagLabelInterested.Visible = true;
            indicateFlagLabelInterestedNotInterested.Visible = true;
            proposalInfoLabel.Visible = true;

            PopulateCommentsAndInterestLabel(Convert.ToInt32(proposalsList.SelectedItem.Value));

            //decide the state (enable/disable) of buttons
            DecideLikeDislikeEnable(Convert.ToInt32(proposalsList.SelectedItem.Value));
            DecideFlagEnabledOrDisabled(Convert.ToInt32(proposalsList.SelectedItem.Value));
        }
        
    }
}
